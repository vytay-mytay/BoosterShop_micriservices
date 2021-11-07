using shop.Domain.Entities;
using shop.Models.ResponseModels;

namespace shop.Services.StartApp
{
    public class AutoMapperProfileConfiguration : AutoMapper.Profile
    {
        public AutoMapperProfileConfiguration()
        : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            #region User model

            CreateMap<Product, ProductResponseModel>();

            CreateMap<OrderProduct, ProductResponseModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.ProductId))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Product != null ? x.Product.Name : null))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Product != null ? x.Product.Price : 0))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(x => x.Quantity));

            CreateMap<Order, OrderResponseModel>()
                .ForMember(x => x.TotalPrice, opt => opt.MapFrom(x => x.SumPrice))
                .ForMember(x => x.Products, opt => opt.MapFrom(x => x.OrderProducts));

            #endregion
        }
    }
}
