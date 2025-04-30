using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class ProductTestData
    {
        public static List<Product> GenerateProducts(int count)
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.Rating, f => new Rating
                {
                    Rate = f.Random.Double(1, 5),
                    Count = f.Random.Int(0, 1000)
                });

            return faker.Generate(count);
        }
        public static GetAllProductQuery GenerateValidQuery(int? page = null, int? size = null, string order = "")
        {
            return new GetAllProductQuery
            {
                Page = page ?? 1,
                Size = size ?? 10,
                Order = order
            };
        }
        public static GetAllProductQuery GenerateInvalidQuery()
        {
            return new GetAllProductQuery
            {
                Page = 0,
                Size = 101,
                Order = "invalid_field invalid_direction"
            };
        }
    }
}
