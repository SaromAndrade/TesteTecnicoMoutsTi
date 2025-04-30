using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        public static Sale GenerateEmptySale()
        {
            var user = UserTestData.GenerateValidUser();
            var branch = BranchTestData.GenerateValidBranch();

            return new Sale(user, branch);
        }
        public static Sale GenerateSaleWithSingleItem()
        {
            var sale = GenerateEmptySale();
            var item = SaleItemTestData.GenerateValidSaleItem(2);
            sale.Items.Add(item);
            sale.RecalculateTotals();

            return sale;
        }

        public static Sale GenerateSaleWithMultipleItems()
        {
            var sale = GenerateEmptySale();

            sale.Items.Add(SaleItemTestData.GenerateValidSaleItem(1));
            sale.Items.Add(SaleItemTestData.GenerateSaleItemWithMediumDiscount());
            sale.Items.Add(SaleItemTestData.GenerateSaleItemWithMaxDiscount());

            sale.RecalculateTotals();
            return sale;
        }

        public static Sale GenerateCancelledSale()
        {
            var sale = GenerateSaleWithSingleItem();
            sale.Cancel();
            return sale;
        }
    }
}
