using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validator
{
    public class CancelSaleCommandValidatorTests
    {
        private readonly CancelSaleCommandValidator _validator;

        public CancelSaleCommandValidatorTests()
        {
            _validator = new CancelSaleCommandValidator();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(9999)]
        [InlineData(int.MaxValue)]
        public void Validate_ShouldPass_WhenSaleNumberIsValid(int saleNumber)
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = saleNumber };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Validate_ShouldFail_WhenSaleNumberIsInvalid(int saleNumber)
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = saleNumber };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "SaleNumber");
        }

        [Fact]
        public void Validate_ShouldReturnCorrectErrorMessage_WhenSaleNumberIsZero()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 0 };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                  .Which.ErrorMessage.Should().Be("SaleNumber must be greater than zero.");
        }
    }
}
