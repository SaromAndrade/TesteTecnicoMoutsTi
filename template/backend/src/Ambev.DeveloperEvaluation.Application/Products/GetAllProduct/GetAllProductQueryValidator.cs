using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductQueryValidator : AbstractValidator<GetAllProductQuery>
    {
        public GetAllProductQueryValidator()
        {
            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(query => query.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must be less than or equal to 100.");

            RuleFor(query => query.Order)
                .Must(BeAValidOrderExpression).When(query => !string.IsNullOrEmpty(query.Order))
                .WithMessage("Order must be in the format 'field [asc|desc]' (e.g., 'price desc').");
        }
        private bool BeAValidOrderExpression(string order)
        {
            var orderParams = order.Split(',');

            foreach (var param in orderParams)
            {
                var orderBy = param.Trim().Split(' ');
                if (orderBy.Length < 1 || orderBy.Length > 2)
                    return false;

                var property = orderBy[0];
                var direction = orderBy.Length > 1 ? orderBy[1] : "asc";

                if (!IsValidProperty(property))
                    return false;

                if (!IsValidDirection(direction))
                    return false;
            }

            return true;
        }
        private bool IsValidProperty(string property)
        {
            var validProperties = new[] { "name", "price" }; 
            return validProperties.Contains(property.ToLower());
        }
        private bool IsValidDirection(string direction)
        {
            var validDirections = new[] { "asc", "desc" };
            return validDirections.Contains(direction.ToLower());
        }
    }
}
