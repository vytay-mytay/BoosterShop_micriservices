using authentication.DAL.Abstract;
using authentication.Domain.Entities.Identity;
using authentication.Helpers.Attributes;
using authentication.Models.RequestModels.Test;
using authentication.Models.ResponseModels;
using authentication.Models.ResponseModels.Session;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace authentication.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [Validate]
    public class TestController : _BaseApiController
    {
        private ILogger<TestController> _logger;
        private IUnitOfWork _unitOfWork;
        private IJWTService _jwtService;

        public TestController(ILogger<TestController> logger,
            IUnitOfWork unitOfWork,
            IJWTService jwtService)
            : base()
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        /// <summary>
        /// For Swagger UI
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("authorize")]
        public async Task<IActionResult> AuthorizeWithoutCredentials([FromBody] ShortAuthorizationRequestModel model)
        {
            IQueryable<ApplicationUser> users = null;

            if (model.Id.HasValue)
                users = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == model.Id);
            else if (!string.IsNullOrEmpty(model.UserName))
                users = _unitOfWork.Repository<ApplicationUser>().Get(x => x.UserName == model.UserName);

            var user = await users.FirstOrDefaultAsync();

            if (user == null)
            {
                Errors.AddError("", "User is not found");
                return Errors.Error(HttpStatusCode.NotFound);
            }

            return Json(new JsonResponse<LoginResponseModel>(await _jwtService.BuildLoginResponse(user)));
        }
    }
}