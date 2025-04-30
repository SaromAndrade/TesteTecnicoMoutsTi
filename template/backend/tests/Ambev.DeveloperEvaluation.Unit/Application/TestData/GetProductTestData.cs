using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class GetProductTestData
    {
        public static GetProductQuery GenerateValidQuery()
        {
            return new GetProductQuery { Id = Guid.NewGuid() };
        }
        public static GetProductQuery GenerateInvalidQuery()
        {
            return new GetProductQuery { Id = Guid.Empty };
        }

        public static Product GenerateSampleProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Rating = new Rating { Rate = 4.5, Count = 100 }
            };
        }
    }
}
