using authentication.Models.RequestModels;
using authentication.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace authentication.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RefreshTokenRoleValidationAttribute : ActionFilterAttribute
    {
        private List<string> _roles;
        private ErrorResponseModel _errors;

        public RefreshTokenRoleValidationAttribute(string[] allowedRoles)
        {
            _roles = allowedRoles.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments["model"] as RefreshTokenRequestModel;

            try
            {
                var decodedJwt = new JwtSecurityTokenHandler().ReadJwtToken(model.RefreshToken);

                if (decodedJwt.Claims.Select(x => x.Type == ClaimsIdentity.DefaultRoleClaimType && !_roles.Contains(x.Value)).Any())
                {
                    _errors = new ErrorResponseModel();
                    context.Result = _errors.Error(System.Net.HttpStatusCode.Forbidden);

                    return;
                }

            }
            catch
            {
                _errors = new ErrorResponseModel();
                context.Result = _errors.BadRequest("refreshToken", "Refresh token is invalid");
            }

            return;
        }
    }
}
