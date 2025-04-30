using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CancelSaleTestData
    {
        public static CancelSaleCommand GenerateValidCommand()
        {
            return new CancelSaleCommand { SaleNumber = 12345 };
        }

        public static CancelSaleCommand GenerateInvalidCommand()
        {
            return new CancelSaleCommand { SaleNumber = 0 };
        }

        public static Sale GenerateSampleSale()
        {
            var sale = new Sale(UserTestData.GenerateValidUser(), BranchTestData.GenerateValidBranch());
            sale.Id = Guid.NewGuid();
            return sale;
        }
    }
}
