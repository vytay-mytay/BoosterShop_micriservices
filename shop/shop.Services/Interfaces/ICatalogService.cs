using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using System.Threading.Tasks;

namespace shop.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<PaginationResponseModel<ProductResponseModel>> GetProductsCatalog(PaginationBaseRequestModel model);
    }
}
