using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID cannot be empty.");

            RuleFor(x => x.CustomerEmail)
                .NotEmpty()
                .WithMessage("CustomerEmail cannot be empty.")
                .EmailAddress()
                .WithMessage("CustomerEmail must be a valid email address.");

            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("Branch ID cannot be empty.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status must be a valid SaleStatus value.");

            RuleFor(command => command.Items)
                .NotEmpty()
                .WithMessage("SaleItems list cannot be empty.")
                .Must(items => items.All(p => p != null))
                .WithMessage("SaleItems list cannot contain null values.")
                .Must(items => items.All(x => x.Quantity > 0))
                .WithMessage("All SaleItems must have a quantity greater than zero.")
                .Must(items => items.GroupBy(x => x.ProductId).All(g => g.Count() == 1))
                .WithMessage("Duplicate product items are not allowed.");
        }
    }
}
