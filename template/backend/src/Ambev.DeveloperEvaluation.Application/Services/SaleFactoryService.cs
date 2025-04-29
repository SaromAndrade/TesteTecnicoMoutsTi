using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleFactoryService : ISaleFactoryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;

        public SaleFactoryService(IUserRepository userRepository, IBranchRepository branchRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
        }
        public async Task<Sale> CreateSaleAsync(SaleDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(dto.CustomerEmail, cancellationToken);
            var branch = await _branchRepository.GetByIdAsync(dto.BranchId, cancellationToken);

            var sale = new Sale(user, branch);

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                sale.AddItem(product, item.Quantity);
            }

            return sale;
        }
    }
}
