using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateSaleItem()
        {
            var item = SaleItemTestData.GenerateValidSaleItem(2);

            item.Should().NotBeNull();
            item.Quantity.Should().Be(2);
            item.Product.Should().NotBeNull();
            item.TotalPrice.Should().Be(item.Product.Price * 2);
        }
        [Fact]
        public void Constructor_WithQuantityGreaterThan20_ShouldThrowException()
        {
            var product = ProductTestData.GenerateValidProduct();
            Action act = () => new SaleItem(product, 21);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Quantity cannot exceed 20 items for the same product");
        }

        [Fact]
        public void SaleItem_WithQuantityLessThan4_ShouldHaveNoDiscount()
        {
            var item = SaleItemTestData.GenerateValidSaleItem(2);

            item.DiscountAmount.Should().Be(0);
            item.FinalPrice.Should().Be(item.TotalPrice);
        }

        [Fact]
        public void SaleItem_WithQuantityBetween4And9_ShouldHave10PercentDiscount()
        {
            var item = SaleItemTestData.GenerateSaleItemWithMediumDiscount();
            var expectedDiscount = item.TotalPrice * 0.1m;

            item.DiscountAmount.Should().Be(expectedDiscount);
            item.FinalPrice.Should().Be(item.TotalPrice - expectedDiscount);
        }

        [Fact]
        public void SaleItem_WithQuantityBetween10And20_ShouldHave20PercentDiscount()
        {
            var item = SaleItemTestData.GenerateSaleItemWithMaxDiscount();
            var expectedDiscount = item.TotalPrice * 0.2m;

            item.DiscountAmount.Should().Be(expectedDiscount);
            item.FinalPrice.Should().Be(item.TotalPrice - expectedDiscount);
        }

        [Fact]
        public void IncreaseQuantity_WithValidAdditional_ShouldUpdateQuantityAndDiscount()
        {
            var item = SaleItemTestData.GenerateValidSaleItem(5);
            item.IncreaseQuantity(5);

            item.Quantity.Should().Be(10);
            var expectedDiscount = item.TotalPrice * 0.2m;
            item.DiscountAmount.Should().Be(expectedDiscount);
            item.FinalPrice.Should().Be(item.TotalPrice - expectedDiscount);
        }

        [Fact]
        public void IncreaseQuantity_WithNonPositiveValue_ShouldThrowException()
        {
            var item = SaleItemTestData.GenerateValidSaleItem();

            Action act = () => item.IncreaseQuantity(0);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Additional quantity must be positive");
        }

        [Fact]
        public void IncreaseQuantity_BeyondLimit_ShouldThrowException()
        {
            var item = SaleItemTestData.GenerateSaleItemAtLimitQuantity();

            Action act = () => item.IncreaseQuantity(1);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Cannot have more than 20 items of the same product");
        }
    }
}
