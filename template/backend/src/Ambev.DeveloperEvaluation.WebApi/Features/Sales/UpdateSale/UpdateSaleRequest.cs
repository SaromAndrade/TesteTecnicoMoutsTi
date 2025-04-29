using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        public string CustomerEmail { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; }
        public SaleStatus Status { get; set; }
    }
}
