using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
    {
        public CancelSaleRequestValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status must be a valid value: Created, Completed, or Cancelled.");
            RuleFor(command => command.SaleNumber)
               .GreaterThan(0).WithMessage("SaleNumber must be greater than zero.");
        }
    }
}
