using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
       
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<(List<Product> Products, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
