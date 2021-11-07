using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shop.DAL.Abstract;
using shop.Domain.Entities;
using shop.Models.RequestModels;
using shop.Models.ResponseModels;
using shop.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Services.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CatalogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseModel<ProductResponseModel>> GetProductsCatalog(PaginationBaseRequestModel model)
        {
            var products = _unitOfWork.Repository<Product>().Get(x => x.Name != null).AsNoTracking();

            var count = products.Count();

            var responseProducts = _mapper.Map<List<ProductResponseModel>>(products.Skip(model.Offset).Take(model.Limit).ToList());

            var response = new PaginationResponseModel<ProductResponseModel>()
            {
                Data = responseProducts,
                TotalCount = count,
            };

            return response;
        }
    }
}
