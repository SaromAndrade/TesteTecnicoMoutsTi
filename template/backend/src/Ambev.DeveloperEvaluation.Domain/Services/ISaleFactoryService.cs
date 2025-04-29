using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleFactoryService
    {
        Task<Sale> CreateSaleAsync(SaleDto request, CancellationToken cancellationToken);
    }
}
