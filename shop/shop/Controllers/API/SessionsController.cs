//using shop.Helpers.Attributes;

//namespace shop.Controllers.API
//{
//    [ApiController]
//    [ApiVersion("1.0")]
//    [Produces("application/json")]
//    [Route("api/v{api-version:apiVersion}/[controller]")]
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.User)]
//    [Validate]
//    public class SessionsController : _BaseApiController
//    {
//        private IAccountService _accountService;

//        public SessionsController(IAccountService accountService)
//              : base()
//        {
//            _accountService = accountService;
//        }

//        // POST api/v1/sessions
//        /// <summary>
//        /// Login User. 'accessTokenLifetime' - access token life time (sec)
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST api/v1/sessions
//        ///     {                
//        ///         "email" : "test@email.com",
//        ///         "password" : "1simplepassword",
//        ///         "accessTokenLifetime": "60" 
//        ///     }
//        ///
//        /// </remarks>
//        /// <returns>A user info with an HTTP 200, or errors with an HTTP 500.</returns>  
//        [AllowAnonymous]
//        [PreventSpam(Name = "Login", Seconds = 1)]
//        [SwaggerResponse(200, ResponseMessages.SuccessfulLogin, typeof(JsonResponse<LoginResponseModel>))]
//        [SwaggerResponse(400, ResponseMessages.InvalidData, typeof(ErrorResponseModel))]
//        [SwaggerResponse(403, ResponseMessages.AccountBlocked, typeof(ErrorResponseModel))]
//        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
//        {
//            var response = await _accountService.Login(model);

//            return Json(new JsonResponse<LoginResponseModel>(response));
//        }

//        // DELETE api/v1/sessions
//        /// <summary>
//        /// Clears user tokens
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     DELETE api/v1/sessions
//        ///
//        /// </remarks>
//        /// <returns>HTTP 200 on successful logout, or errors with an HTTP 500.</returns>
//        /// <response code="200">Logout successful</response>
//        /// <response code="401">Unauthorized</response>   
//        /// <response code="404">If the user is not found</response>  
//        /// <response code="500">Internal server error</response>  
//        [HttpDelete]
//        [PreventSpam(Name = "Logout")]
//        [SwaggerResponse(200, ResponseMessages.RequestSuccessful, typeof(JsonResponse<MessageResponseModel>))]
//        [SwaggerResponse(401, ResponseMessages.Unauthorized, typeof(ErrorResponseModel))]
//        [SwaggerResponse(404, ResponseMessages.NotFound, typeof(ErrorResponseModel))]
//        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
//        public async Task<IActionResult> Logout()
//        {
//            await _accountService.Logout(User.GetUserId());

//            return Json(new JsonResponse<MessageResponseModel>(new MessageResponseModel("You have been logged out")));
//        }

//        // PUT api/v1/sessions
//        /// <summary>
//        /// Refresh user access token
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     PUT api/v1/sessions
//        ///     {                
//        ///         "refreshToken" : "some token"
//        ///     }
//        ///
//        /// </remarks>
//        /// <returns>A user Token with an HTTP 200, or errors with an HTTP 500.</returns> 
//        [AllowAnonymous]
//        [PreventSpam(Name = "RefreshToken")]
//        [RefreshTokenRoleValidation(new[] { Role.User })]
//        [ProducesResponseType(typeof(JsonResponse<TokenResponseModel>), 200)]
//        [SwaggerResponse(200, ResponseMessages.RequestSuccessful, typeof(JsonResponse<TokenResponseModel>))]
//        [SwaggerResponse(400, ResponseMessages.InvalidData, typeof(ErrorResponseModel))]
//        [SwaggerResponse(403, ResponseMessages.Forbidden, typeof(ErrorResponseModel))]
//        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
//        [HttpPut]
//        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel model)
//        {
//            var response = await _accountService.RefreshTokenAsync(model.RefreshToken, new List<string> { Role.User });

//            return Json(new JsonResponse<TokenResponseModel>(response));
//        }
//    }
//}