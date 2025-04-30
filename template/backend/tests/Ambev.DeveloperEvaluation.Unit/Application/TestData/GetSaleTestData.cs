using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class GetSaleTestData
    {
        public static GetSaleQuery GenerateValidQuery()
        {
            return new GetSaleQuery { Id = Guid.NewGuid() };
        }

        public static GetSaleQuery GenerateInvalidQuery()
        {
            return new GetSaleQuery { Id = Guid.Empty };
        }

        public static GetSaleResult GenerateExpectedResult(Sale sale)
        {
            return new GetSaleResult
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
