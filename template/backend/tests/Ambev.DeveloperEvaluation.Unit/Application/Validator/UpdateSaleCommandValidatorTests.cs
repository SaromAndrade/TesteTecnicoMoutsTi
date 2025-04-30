using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public class UpdateSaleCommandValidatorTests
    {
        private readonly UpdateSaleCommandValidator _validator;

        public UpdateSaleCommandValidatorTests()
        {
            _validator = new UpdateSaleCommandValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_ShouldFail_WhenCustomerEmailIsInvalid(string invalidEmail)
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            command.CustomerEmail = invalidEmail;

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CustomerEmail");
        }

        [Fact]
        public void Validate_ShouldFail_WhenBranchIdIsEmpty()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            command.BranchId = Guid.Empty;

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "BranchId");
        }

        [Fact]
        public void Validate_ShouldFail_WhenItemsListIsEmpty()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            command.Items = new List<SaleItemDto>();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items");
        }

        [Fact]
        public void Validate_ShouldFail_WhenItemsContainZeroQuantity()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            command.Items[0].Quantity = 0;

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items");
        }
    }
}
