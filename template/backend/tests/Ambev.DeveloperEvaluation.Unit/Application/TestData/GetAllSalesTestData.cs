using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class GetAllSalesTestData
    {
        public static GetAllSalesQuery GenerateValidQuery(int? page = null, int? size = null, string order = "")
        {
            return new GetAllSalesQuery
            {
                Page = page ?? 1,
                Size = size ?? 10,
                Order = order
            };
        }
        public static List<Sale> GenerateSalesList(int count)
        {
            var result = new List<Sale>();

            for (int i = 0; i < count; i++)
            {
                if (i % 3 == 0)
                    result.Add(SaleTestData.GenerateEmptySale());
                else if (i % 3 == 1)
                    result.Add(SaleTestData.GenerateSaleWithSingleItem());
                else
                    result.Add(SaleTestData.GenerateSaleWithMultipleItems());
            }

            return result;
        }
    }
}
