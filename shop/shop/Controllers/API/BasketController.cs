using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop.Common.Constants;
using shop.Helpers.Attributes;
using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using shop.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace shop.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiversion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Validate]
    public class BasketController : _BaseApiController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // GET api/v1/Basket
        /// <summary>
        /// Get user basket
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/basket
        ///
        /// </remarks>
        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<OrderResponseModel>))]
        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        [HttpGet]
        [Validate]
        public async Task<IActionResult> Basket()
        {
            var result = await _basketService.GetBasket(1);

            return Json(new JsonResponse<OrderResponseModel>(result));
        }

        // PATCH api/v1/Basket
        /// <summary>
        /// Add product to user basket
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH api/v1/basket
        ///     {
        ///         productId: 1,
        ///         quantity: 1
        ///     }
        ///
        /// </remarks>
        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<MessageResponseModel>))]
        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        [HttpPatch]
        [Validate]
        public async Task<IActionResult> AddToBasket([FromBody] BasketRequestModel model)
        {
            await _basketService.AddToBasket(1, model);

            return Json(new JsonResponse<MessageResponseModel>(new MessageResponseModel("Product has been add to basket")));
        }

        // DELETE api/v1/Basket
        /// <summary>
        /// Clear user basket
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/v1/basket
        ///
        /// </remarks>
        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<MessageResponseModel>))]
        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        [HttpDelete]
        [Validate]
        public async Task<IActionResult> ClearBasket()
        {
            await _basketService.ClearBasket(1);

            return Json(new JsonResponse<MessageResponseModel>(new MessageResponseModel("Basket has been emptied")));
        }

        // POST api/v1/Basket
        /// <summary>
        /// Buy products in basket
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/basket
        ///
        /// </remarks>
        /// <returns>User email and info about email status, or errors with an HTTP 4xx or 500 code.</returns>
        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonResponse<MessageResponseModel>))]
        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        [HttpPost]
        [Validate]
        public async Task<IActionResult> BuyBasket()
        {
            await _basketService.BuyBasket(1);

            return Json(new JsonResponse<MessageResponseModel>(new MessageResponseModel("Congratulations on your purchase!")));
        }

    }
}
