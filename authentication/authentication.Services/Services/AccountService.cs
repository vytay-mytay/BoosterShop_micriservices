using authentication.Common.Exceptions;
using authentication.Common.Utilities;
using authentication.DAL.Abstract;
using authentication.Domain.Entities.Identity;
using authentication.Models.RequestModels;
using authentication.Models.ResponseModels.Session;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace authentication.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HashUtility _hashUtility;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager, HashUtility hashUtility, IUnitOfWork unitOfWork, IJWTService jwtService,
            IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _hashUtility = hashUtility;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponseModel> Register(RegisterRequestModel model)
        {
            model.Email = model.Email.Trim().ToLower();

            ApplicationUser user = _unitOfWork.Repository<ApplicationUser>().Find(x => x.Email.ToLower() == model.Email);

            if (user != null && user.EmailConfirmed)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "email", "Email is already registered");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    IsActive = true,
                    RegistratedAt = DateTime.UtcNow,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    throw new CustomException(HttpStatusCode.BadRequest, "general", result.Errors.FirstOrDefault().Description);

                result = await _userManager.AddToRoleAsync(user, Role.User);

                if (!result.Succeeded)
                    throw new CustomException(HttpStatusCode.BadRequest, "general", result.Errors.FirstOrDefault().Description);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmResult = await _userManager.ConfirmEmailAsync(user, code);

                if (!confirmResult.Succeeded)
                    throw new CustomException(HttpStatusCode.BadRequest, "token", "The verification link is not active");
            }

            return await _jwtService.BuildLoginResponse(user);
        }

        public async Task<LoginResponseModel> Login(LoginRequestModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Email == model.Email)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefault();

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) || !user.UserRoles.Any(x => x.Role.Name == Role.User))
                throw new CustomException(HttpStatusCode.BadRequest, "credentials", "Invalid credentials");

            if (!string.IsNullOrEmpty(model.Email) && !user.EmailConfirmed)
                throw new CustomException(HttpStatusCode.BadRequest, "email", "Email is not confirmed");

            if (user.IsDeleted)
                throw new CustomException(HttpStatusCode.BadRequest, "general", "Your account was deleted by admin, to know more please contact administration.");

            if (!user.IsActive)
                throw new CustomException(HttpStatusCode.Forbidden, "general", "Your account was blocked. For more information please email to following address: ");

            return await _jwtService.BuildLoginResponse(user, model.AccessTokenLifetime);
        }

        public async Task<TokenResponseModel> RefreshTokenAsync(string refreshToken, List<string> roles)
        {
            var token = _unitOfWork.Repository<UserToken>().Get(w => w.RefreshTokenHash == _hashUtility.GetHash(refreshToken) && w.IsActive && w.RefreshExpiresDate > DateTime.UtcNow)
                .TagWith(nameof(RefreshTokenAsync) + "_GetRefreshToken")
                .Include(x => x.User)
                    .ThenInclude(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                .FirstOrDefault();

            if (token == null)
                throw new CustomException(HttpStatusCode.BadRequest, "refreshToken", "Refresh token is invalid");

            if (!token.User.UserRoles.Any(x => roles.Contains(x.Role.Name)))
                throw new CustomException(HttpStatusCode.Forbidden, "role", "Access denied");

            var result = await _jwtService.CreateUserTokenAsync(token.User, isRefresh: true);
            _unitOfWork.SaveChanges();

            return result;
        }

        public async Task Logout(int userId)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == userId)
                    .TagWith(nameof(Logout) + "_GetUser")
                    .Include(x => x.Tokens)
                    .FirstOrDefault();

            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "user", "User is not found");

            await _jwtService.ClearUserTokens(user);
        }

        public async Task<bool> Verify(string token)
        {
            var hash = _hashUtility.GetHash(token);
            return _unitOfWork.Repository<UserToken>().Find(t => t.AccessTokenHash == hash && t.IsActive) != null;
        }
    }
}
