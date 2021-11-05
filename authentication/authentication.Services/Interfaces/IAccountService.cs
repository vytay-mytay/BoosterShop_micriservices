using authentication.Models.RequestModels;
using authentication.Models.ResponseModels.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace authentication.Services.Interfaces
{
    public interface IAccountService
    {
        Task<TokenResponseModel> RefreshTokenAsync(string refreshToken, List<string> roles);

        Task<LoginResponseModel> Register(RegisterRequestModel model);

        Task<LoginResponseModel> Login(LoginRequestModel model);

        Task Logout(int userId);

        Task<bool> Verify(string token);
    }
}
