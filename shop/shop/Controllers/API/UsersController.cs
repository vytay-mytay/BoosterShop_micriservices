//using shop.Common.Constants;
//using shop.DAL.Abstract;
//using shop.Helpers.Attributes;
//using shop.Models.RequestModels;
//using shop.Models.ResponseModels;
//using shop.Models.ResponseModels.Session;
//using shop.Services.Interfaces;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace shop.Controllers.API
//{
//    [ApiController]
//    [ApiVersion("1.0")]
//    [Produces("application/json")]
//    [Route("api/v{api-version:apiVersion}/[controller]")]
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    [Validate]
//    public class UsersController : _BaseApiController
//    {
//        private readonly IAccountService _accountService;
//        private IUnitOfWork _unitOfWork;

//        public UsersController(IAccountService accountService, IUnitOfWork unitOfWork)
//             : base()
//        {
//            _accountService = accountService;
//            _unitOfWork = unitOfWork;
//        }

//        // POST api/v1/users
//        /// <summary>
//        /// Register new user.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST api/v1/users
//        ///     {                
//        ///         "email" : "test@example.com",
//        ///         "password" : "1simplepassword",
//        ///         "confirmPassword" : "1simplepassword"
//        ///     }
//        ///
//        /// </remarks>
//        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
//        [AllowAnonymous]
//        [PreventSpam(Name = "Register")]
//        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<LoginResponseModel>))]
//        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
//        [SwaggerResponse(422, ResponseMessages.EmailAlreadyRegistered, typeof(ErrorResponseModel))]
//        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
//        [HttpPost]
//        [Validate]
//        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
//        {
//            var response = await _accountService.Register(model);

//            return Json(new JsonResponse<LoginResponseModel>(response));
//        }

//        // GET api/v1/users/verify
//        /// <summary>
//        /// Verify user.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     GET api/v1/users/verify
//        ///
//        /// </remarks>
//        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
//        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<LoginResponseModel>))]
//        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
//        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
//        [HttpGet("verify")]
//        [Validate]
//        public async Task<IActionResult> Verify()
//        {
//            var authorization = HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

//            var token = authorization.Value.ToString()[7..];

//            var userExists = await _accountService.Verify(token);

//            if (!userExists)
//            {
//                return Unauthorized();
//            }

//            return NoContent();
//        }
//    }
//}