using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
    {
        public DeleteSaleCommandValidator()
        {
            RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("User ID is required");
        }
    }
}
