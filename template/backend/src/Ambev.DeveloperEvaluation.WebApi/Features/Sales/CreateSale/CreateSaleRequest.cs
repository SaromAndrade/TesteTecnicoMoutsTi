using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string CustomerEmail { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }
}
