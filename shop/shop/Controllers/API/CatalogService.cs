using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop.Common.Constants;
using shop.Helpers.Attributes;
using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using shop.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shop.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiversion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Validate]
    public class CatalogService : _BaseApiController
    {
        private readonly ICatalogService _catalogService;

        public CatalogService(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        //GET api/v1/catalog
        /// <summary>
        /// Get products catalog
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/catalog
        ///
        /// </remarks>
        /// <returns>Catalog list, or errors with an HTTP 4xx or 500 code.</returns>
        [SwaggerResponse(200, ResponseMessages.SuccessfulRegistration, typeof(JsonPaginationResponse<List<ProductResponseModel>>))]
        [SwaggerResponse(400, ResponseMessages.InvalidCredentials, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        [HttpGet]
        public async Task<IActionResult> Catalog([FromQuery] PaginationBaseRequestModel model)
        {
            var result = await _catalogService.GetProductsCatalog(model);

            return Json(new JsonPaginationResponse<List<ProductResponseModel>>(result.Data, model.Limit + model.Offset, result.TotalCount));
        }
    }
}
