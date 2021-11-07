using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shop.Common.Exceptions;
using shop.DAL.Abstract;
using shop.Domain.Entities;
using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using shop.Services.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace shop.Services.Services
{
    public class BasketService : IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderResponseModel> GetBasket(int userId)
        {
            var order = _unitOfWork.Repository<Order>().Get(x => x.UserId == userId)
                                                        .Include(x => x.OrderProducts)
                                                            .ThenInclude(x => x.Product)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            if (order == null)
                throw new CustomException(HttpStatusCode.NotFound, "order", "Basket is empty");

            var response = _mapper.Map<OrderResponseModel>(order);

            return response;
        }

        public async Task AddToBasket(int userId, BasketRequestModel model)
        {
            var product = _unitOfWork.Repository<Product>().Find(x => x.Id == model.ProductId);

            if (product == null)
                throw new CustomException(HttpStatusCode.NotFound, "productId", "Product not found");

            if (product.Quantity < model.Quantity)
                throw new CustomException(HttpStatusCode.BadRequest, "quantity", "To much Quantity");

            var order = _unitOfWork.Repository<Order>().Find(x => x.UserId == userId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                };
            }

            var orderProduct = _unitOfWork.Repository<OrderProduct>().Find(x => x.OrderId == order.Id && x.ProductId == product.Id);

            if (orderProduct == null)
            {
                orderProduct = new OrderProduct()
                {
                    ProductId = product.Id,
                    Quantity = model.Quantity,
                };
            }

            order.OrderProducts.Add(orderProduct);
            order.SumPrice += product.Price * model.Quantity;

            if (order.Id == 0)
                _unitOfWork.Repository<Order>().Insert(order);

            _unitOfWork.SaveChanges();
        }

        public async Task ClearBasket(int userId)
        {
            var order = _unitOfWork.Repository<Order>().Find(x => x.UserId == userId);

            if (order == null)
                throw new CustomException(HttpStatusCode.NotFound, "order", "Basket is already empty");

            _unitOfWork.Repository<Order>().Delete(order);

            _unitOfWork.SaveChanges();
        }

        public async Task BuyBasket(int userId)
        {
            var order = _unitOfWork.Repository<Order>().Get(x => x.UserId == userId)
                                                        .Include(x => x.OrderProducts)
                                                            .ThenInclude(x => x.Product)
                                                        .FirstOrDefault();

            if (order == null)
                throw new CustomException(HttpStatusCode.NotFound, "order", "Basket is empty");

            if (order.OrderProducts.Any(x => x.Quantity > x.Product.Quantity))
                throw new CustomException(HttpStatusCode.BadRequest, "quantity", "Invalid quantity");

            foreach (var orderProduct in order.OrderProducts)
                orderProduct.Product.Quantity -= orderProduct.Quantity;

            _unitOfWork.SaveChanges();

            await ClearBasket(userId);
        }
    }
}
