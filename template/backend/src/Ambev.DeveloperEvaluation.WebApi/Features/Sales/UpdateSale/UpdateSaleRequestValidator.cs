using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("A valid customer email is required.");

            RuleFor(command => command.Items)
                .NotEmpty().WithMessage("Items list cannot be empty.")
                .Must(products => products.All(p => p != null)).WithMessage("Items list cannot contain null values.")
                .Must(p => p.All(x => x.Quantity > 0)).WithMessage("All Items must have a quantity greater than zero.");
        }
    }
}
