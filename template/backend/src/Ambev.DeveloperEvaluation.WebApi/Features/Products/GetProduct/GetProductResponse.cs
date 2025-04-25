using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductResponse
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; set; }
        public Rating? Rating { get; set; }
    }
}
