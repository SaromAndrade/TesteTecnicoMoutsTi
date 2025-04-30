using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleTestData
    {
        public static CreateSaleCommand GenerateValidCommand()
        {
            return new CreateSaleCommand
            {
                CustomerEmail = "customer@example.com",
                BranchId = Guid.NewGuid(),
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 2 },
                    new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 3 }
                }
            };
        }

        public static CreateSaleCommand GenerateInvalidCommand()
        {
            return new CreateSaleCommand
            {
                CustomerEmail = "",
                BranchId = Guid.Empty,
                Items = new List<SaleItemDto>()
            };
        }

        public static Sale GenerateSampleSale()
        {
            var user = UserTestData.GenerateValidUser();
            var branch = BranchTestData.GenerateValidBranch();
            var sale = new Sale(user, branch);

            sale.AddItem(ProductTestData.GenerateProducts(1).First(), 2);
            sale.AddItem(ProductTestData.GenerateProducts(1).First(), 3);

            return sale;
        }

        public static SaleDto GenerateValidSaleDto()
        {
            return new SaleDto
            {
                CustomerId = Guid.NewGuid(),
                CustomerEmail = "customer@example.com",
                BranchId = Guid.NewGuid(),
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 2 },
                    new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 3 }
                }
            };
        }
    }
}
