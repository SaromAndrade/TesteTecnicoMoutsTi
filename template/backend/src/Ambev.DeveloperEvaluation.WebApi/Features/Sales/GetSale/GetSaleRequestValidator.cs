using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
    {
        public GetSaleRequestValidator()
        {
            RuleFor(request => request.Id)
                .Must(id => id != Guid.Empty)
                .WithMessage("O Id informado não pode ser vazio.");
        }
    }
}
