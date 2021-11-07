using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using System.Threading.Tasks;

namespace shop.Services.Interfaces
{
    public interface IBasketService
    {
        Task<OrderResponseModel> GetBasket(int userId);

        Task AddToBasket(int userId, BasketRequestModel model);

        Task ClearBasket(int userId);

        Task BuyBasket(int userId);
    }
}
