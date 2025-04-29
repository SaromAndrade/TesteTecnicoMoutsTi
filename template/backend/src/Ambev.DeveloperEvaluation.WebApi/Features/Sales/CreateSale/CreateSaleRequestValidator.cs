using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.CustomerEmail)
               .NotEmpty().WithMessage("Customer email is required.")
               .EmailAddress().WithMessage("A valid customer email is required.");

            // Branch ID validation
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Branch ID cannot be empty.");


            RuleFor(command => command.Items)
               .NotEmpty().WithMessage("SaleItems list cannot be empty.")
               .Must(products => products.All(p => p != null)).WithMessage("SaleItems list cannot contain null values.")
               .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All SaleItems must have a quantity greater than zero.");
        }
    }
}
