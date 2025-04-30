using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class DeleteSaleTestData
    {
        public static DeleteSaleCommand GenerateValidCommand()
        {
            return new DeleteSaleCommand { Id = Guid.NewGuid() };
        }

        public static DeleteSaleCommand GenerateInvalidCommand()
        {
            return new DeleteSaleCommand { Id = Guid.Empty };
        }
    }
}
