using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleValidatorService : ISaleValidatorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;

        public SaleValidatorService(IUserRepository userRepository, IBranchRepository branchRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
        }
        public async Task ValidateAsync(SaleDto dto, CancellationToken cancellationToken)
        {
            var branchExists = await _branchRepository.ExistsAsync(dto.BranchId, cancellationToken);
            if (!branchExists)
                throw new DomainException("Branch not found");

            var userExists = await _userRepository.ExistsAsync(dto.CustomerEmail, cancellationToken);
            if (!userExists)
                throw new DomainException("User not found");

            foreach (var item in dto.Items)
            {
                var productExists = await _productRepository.ExistsAsync(item.ProductId, cancellationToken);
                if (!productExists)
                    throw new DomainException($"Product {item.ProductId} not found");
            }
        }
    }
}
