using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductProfile : Profile
    {
        public GetProductProfile()
        {
            CreateMap<GetProductRequest, GetProductQuery>();
            CreateMap<GetProductResult, GetProductResponse>();
        }
    }
}
