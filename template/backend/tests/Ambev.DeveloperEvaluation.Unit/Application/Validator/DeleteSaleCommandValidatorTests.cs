using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
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
    public class DeleteSaleCommandValidatorTests
    {
        private readonly DeleteSaleCommandValidator _validator;

        public DeleteSaleCommandValidatorTests()
        {
            _validator = new DeleteSaleCommandValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenIdIsValid()
        {
            // Arrange
            var command = DeleteSaleTestData.GenerateValidCommand();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")] // Guid.Empty
        public void Validate_ShouldFail_WhenIdIsInvalid(string invalidId)
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = Guid.Parse(invalidId) };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void Validate_ShouldReturnCorrectErrorMessage_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = Guid.Empty };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                  .Which.ErrorMessage.Should().Be("User ID is required");
        }
    }
}
