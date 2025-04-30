using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
       
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<(List<Product> Products, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
