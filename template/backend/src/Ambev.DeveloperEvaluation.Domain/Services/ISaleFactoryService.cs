using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleFactoryService
    {
        Task<Sale> CreateSaleAsync(SaleDto request, CancellationToken cancellationToken);
    }
}
