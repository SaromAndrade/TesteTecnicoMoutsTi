using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken);
        Task<(List<Sale> sales, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Sale> UpdateAsync(Guid id, Sale sale, CancellationToken cancellationToken);
        Task<bool> CancelAsync(Guid id, CancellationToken cancellationToken);

    }
}
