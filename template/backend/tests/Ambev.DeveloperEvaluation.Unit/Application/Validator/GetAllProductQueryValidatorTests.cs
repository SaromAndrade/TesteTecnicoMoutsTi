using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validator
{
    public class GetAllProductQueryValidatorTests
    {
        private readonly GetAllProductQueryValidator _validator;
        public GetAllProductQueryValidatorTests()
        {
            _validator = new GetAllProductQueryValidator();
        }
        [Theory]
        [InlineData(1, 10, "", true)]
        [InlineData(1, 100, "name asc", true)]
        [InlineData(0, 10, "", false)]
        [InlineData(1, 101, "", false)]
        [InlineData(1, 10, "invalid_field desc", false)]
        public void Validate_ShouldReturnCorrectResult(int page, int size, string order, bool expectedIsValid)
        {
            // Arrange
            var query = new GetAllProductQuery { Page = page, Size = size, Order = order };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().Be(expectedIsValid);
        }

        [Theory]
        [InlineData("name asc", true)]
        [InlineData("price desc", true)]
        [InlineData("name", true)]
        [InlineData("name asc,price desc", true)]
        [InlineData("invalid_field asc", false)]
        [InlineData("name invalid_direction", false)]
        [InlineData("name asc,invalid_field desc", false)]
        public void Validate_ShouldCorrectlyValidateOrderExpressions(string order, bool expectedIsValid)
        {
            // Arrange
            var query = new GetAllProductQuery { Page = 1, Size = 10, Order = order };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().Be(expectedIsValid);
        }
    }
}
