using Ambev.DeveloperEvaluation.Domain.DTOs;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleValidatorService
    {
        Task ValidateAsync(SaleDto dto, CancellationToken cancellationToken);
    }
}
