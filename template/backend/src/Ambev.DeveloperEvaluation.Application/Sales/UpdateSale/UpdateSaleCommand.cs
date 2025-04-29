using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    { 
        public Guid Id { get; set; }
        public string CustomerEmail { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; }
        public SaleStatus Status { get; set; }
    }
}
