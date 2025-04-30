using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
    public class CreateSaleCommandValidatorTests
    {
        private readonly CreateSaleCommandValidator _validator;
        public CreateSaleCommandValidatorTests()
        {
            _validator = new CreateSaleCommandValidator();
        }
        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateSaleTestData.GenerateValidCommand();

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
            var command = CreateSaleTestData.GenerateValidCommand();
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
            var command = CreateSaleTestData.GenerateValidCommand();
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
            var command = CreateSaleTestData.GenerateValidCommand();
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
            var command = CreateSaleTestData.GenerateValidCommand();
            command.Items[0].Quantity = 0;

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items");
        }
    }
}
