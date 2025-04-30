using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Rating? Rating { get; set; }
    }
}
