using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validator
{
    public class GetProductQueryValidatorTests
    {
        private readonly GetProductQueryValidator _validator;

        public GetProductQueryValidatorTests()
        {
            _validator = new GetProductQueryValidator();
        }
        [Fact]
        public void Validate_ShouldPass_WhenIdIsValid()
        {
            // Arrange
            var query = GetProductTestData.GenerateValidQuery();

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void Validate_ShouldFail_WhenIdIsInvalid(string invalidId)
        {
            // Arrange
            var query = new GetProductQuery { Id = Guid.Parse(invalidId) };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void Validate_ShouldReturnCorrectErrorMessage_WhenIdIsEmpty()
        {
            // Arrange
            var query = new GetProductQuery { Id = Guid.Empty };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "Id" &&
                e.ErrorMessage == "Product ID cannot be empty.");
        }

        [Fact]
        public void Validate_ShouldReturnCorrectErrorMessage_WhenIdIsInvalidFormat()
        {
            // Arrange
            var query = new GetProductQuery { Id = Guid.Empty };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "Id" &&
                e.ErrorMessage == "Product ID cannot be empty.");
        }
    }
}
