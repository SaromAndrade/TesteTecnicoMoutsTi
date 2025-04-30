using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public class UpdateSaleTestData
    {
        public static UpdateSaleCommand GenerateValidCommand()
        {
            return new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                CustomerEmail = "customer@example.com",
                BranchId = Guid.NewGuid(),
                Status = SaleStatus.Created,
                Items = new List<SaleItemDto>
            {
                new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 2 },
                new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 3 }
            }
            };
        }

        public static UpdateSaleCommand GenerateInvalidCommand()
        {
            return new UpdateSaleCommand
            {
                Id = Guid.Empty,
                CustomerEmail = "",
                BranchId = Guid.Empty,
                Items = new List<SaleItemDto>()
            };
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

        public static UpdateSaleResult GenerateExpectedResult(Sale sale)
        {
            return new UpdateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber ?? 0,
                Date = sale.Date,
                CustomerId = sale.Customer.Id,
                BranchId = sale.Branch.Id,
                Items = sale.Items,
                Status = sale.Status,
                TotalAmount = sale.TotalAmount,
                TotalDiscount = sale.TotalDiscount,
                FinalAmount = sale.FinalAmount
            };
        }
    }
}
