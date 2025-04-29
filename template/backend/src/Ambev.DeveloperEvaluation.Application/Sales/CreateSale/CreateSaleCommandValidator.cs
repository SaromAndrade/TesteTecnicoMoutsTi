using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.CustomerEmail)
                .NotEmpty()
                .WithMessage("CustomerEmail cannot be empty.");
            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("Branch ID cannot be empty.");

            RuleFor(command => command.Items)
               .NotEmpty().WithMessage("SaleItems list cannot be empty.")
               .Must(products => products.All(p => p != null)).WithMessage("SaleItems list cannot contain null values.")
               .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All SaleItems must have a quantity greater than zero.");
        }
    }
}
