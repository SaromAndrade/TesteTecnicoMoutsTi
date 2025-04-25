using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; set; }
        public Rating? Rating { get; set; }
    }
}
