using Ambev.DeveloperEvaluation.Application.Branchs.GetAllBranchs;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetAllBranchs
{
    public class GetAllBranchsProfile : Profile
    {
        public GetAllBranchsProfile()
        {
            CreateMap<GetAllBranchsResult, GetAllBranchsResponse>();
        }
    }
}
