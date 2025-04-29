using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleRequest
    {
        public int SaleNumber { get; set; }
        public SaleStatus Status { get; set; }
    }
}
