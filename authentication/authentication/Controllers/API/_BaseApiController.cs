using authentication.Common.Constants;
using authentication.Domain.Entities.Identity;
using authentication.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace authentication.Controllers.API
{
    [ApiVersion("1.0")]
    public class _BaseApiController : Controller
    {
        protected ErrorResponseModel Errors;

        public _BaseApiController()
        {
            Errors = new ErrorResponseModel();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Forbidden()
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(new ErrorResponseModel()
                {
                    Code = ErrorCode.Forbidden,
                }, new JsonSerializerSettings { Formatting = Formatting.Indented }),
                StatusCode = 403,
                ContentType = "application/json"
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> IsAdminAsync(ClaimsPrincipal User)
        {
            return User.IsInRole(Role.Admin);
        }
    }
}