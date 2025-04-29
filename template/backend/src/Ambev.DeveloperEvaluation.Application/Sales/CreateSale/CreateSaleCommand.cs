using Ambev.DeveloperEvaluation.Application.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string CustomerEmail { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }
}
