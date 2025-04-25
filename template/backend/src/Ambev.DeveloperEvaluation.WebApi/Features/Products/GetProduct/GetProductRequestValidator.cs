using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductRequestValidator : AbstractValidator<GetProductRequest>
    {
        public GetProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID cannot be empty.")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Product ID must be a valid GUID.");
        }
    }
}
