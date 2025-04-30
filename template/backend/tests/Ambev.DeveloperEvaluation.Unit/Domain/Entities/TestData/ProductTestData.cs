using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class ProductTestData
    {
        private static readonly Faker<Product> ProductFaker = new Faker<Product>()
                .CustomInstantiator(f => new Product())
                .RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000))
                .RuleFor(p => p.Rating, f => new Rating
                {
                    Rate = f.Random.Double(1, 5),
                    Count = f.Random.Int(0, 1000)
                })
                .FinishWith((f, p) =>
                {
                    p.GetType().GetProperty("Name")?.SetValue(p, f.Commerce.ProductName());
                    p.GetType().GetProperty("Description")?.SetValue(p, f.Lorem.Sentence(10));
                });
        public static Product GenerateValidProduct()
        {
            return ProductFaker.Generate();
        }
        public static Product GenerateProductWithoutRating()
        {
            var product = GenerateValidProduct();
            product.Rating = null;
            return product;
        }
        public static Product GenerateProductWithInvalidPrice()
        {
            var product = GenerateValidProduct();
            product.Price = -10.0m;
            return product;
        }
        public static Product GenerateProductWithEmptyName()
        {
            var product = GenerateValidProduct();
            product.GetType().GetProperty("Name")?.SetValue(product, string.Empty);
            return product;
        }
        public static Rating GenerateInvalidRating()
        {
            return new Rating
            {
                Rate = 6.5,
                Count = -5
            };
        }
    }
}
