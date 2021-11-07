using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shop.Common.Constants;
using shop.Models.ResponseModels;

namespace shop.Controllers.API
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
    }
}