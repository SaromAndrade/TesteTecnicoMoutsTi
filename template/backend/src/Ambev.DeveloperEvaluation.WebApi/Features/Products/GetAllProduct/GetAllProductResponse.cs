using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct
{
    public class GetAllProductResponse
    {
        public List<Product> Data { get; set; }
        public int TotalItems { get; set; }
    }
}
