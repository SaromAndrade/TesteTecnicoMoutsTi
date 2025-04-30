using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
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
    public class SaleTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            var branch = BranchTestData.GenerateValidBranch();

            // Act
            var sale = new Sale(user, branch);

            // Assert
            sale.Id.Should().NotBeEmpty();
            sale.Customer.Should().Be(user);
            sale.Branch.Should().Be(branch);
            sale.Status.Should().Be(SaleStatus.Created);
            sale.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            sale.Items.Should().BeEmpty();
            sale.TotalAmount.Should().Be(0);
            sale.TotalDiscount.Should().Be(0);
            sale.FinalAmount.Should().Be(0);
        }
        [Fact]
        public void AddItem_ShouldAddItemAndRecalculateTotals_WhenSaleIsCreated()
        {
            // Arrange
            var sale = SaleTestData.GenerateEmptySale();
            var product = ProductTestData.GenerateValidProduct();
            int quantity = 2;

            // Act
            sale.AddItem(product, quantity);

            // Assert
            sale.Items.Should().ContainSingle();
            sale.Items[0].Product.Should().Be(product);
            sale.Items[0].Quantity.Should().Be(quantity);
            sale.TotalAmount.Should().Be(product.Price * quantity);
        }
        [Fact]
        public void AddItem_ShouldThrowException_WhenSaleIsCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateCancelledSale();
            var product = ProductTestData.GenerateValidProduct();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.AddItem(product, 1))
                .Message.Should().Be("Cannot add items to a completed or cancelled sale");
        }
        [Fact]
        public void RecalculateTotals_ShouldCalculateCorrectValues_WithSingleItem()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithSingleItem();
            var item = sale.Items[0];
            var expectedTotal = item.TotalPrice;
            var expectedDiscount = item.DiscountAmount;
            var expectedFinal = expectedTotal - expectedDiscount;

            // Act
            sale.RecalculateTotals();

            // Assert
            sale.TotalAmount.Should().Be(expectedTotal);
            sale.TotalDiscount.Should().Be(expectedDiscount);
            sale.FinalAmount.Should().Be(expectedFinal);
        }
        [Fact]
        public void RecalculateTotals_ShouldCalculateCorrectValues_WithMultipleItems()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var expectedTotal = sale.Items.Sum(i => i.TotalPrice);
            var expectedDiscount = sale.Items.Sum(i => i.DiscountAmount);
            var expectedFinal = expectedTotal - expectedDiscount;

            // Act
            sale.RecalculateTotals();

            // Assert
            sale.TotalAmount.Should().Be(expectedTotal);
            sale.TotalDiscount.Should().Be(expectedDiscount);
            sale.FinalAmount.Should().Be(expectedFinal);
        }
        [Fact]
        public void Cancel_ShouldChangeStatusToCancelled_WhenSaleIsCreated()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithSingleItem();

            // Act
            sale.Cancel();

            // Assert
            sale.Status.Should().Be(SaleStatus.Cancelled);
        }

        [Fact]
        public void Cancel_ShouldThrowException_WhenSaleIsAlreadyCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateCancelledSale();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.Cancel())
                .Message.Should().Be("Sale is already cancelled");
        }

        [Fact]
        public void Items_ShouldBeEmpty_WhenSaleIsNew()
        {
            // Arrange & Act
            var sale = SaleTestData.GenerateEmptySale();

            // Assert
            sale.Items.Should().BeEmpty();
        }

        [Fact]
        public void FinalAmount_ShouldBeZero_WhenNoItemsAreAdded()
        {
            // Arrange & Act
            var sale = SaleTestData.GenerateEmptySale();

            // Assert
            sale.FinalAmount.Should().Be(0);
        }

        [Fact]
        public void TotalDiscount_ShouldReflectItemDiscounts()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var expectedDiscount = sale.Items.Sum(i => i.DiscountAmount);

            // Act
            sale.RecalculateTotals();

            // Assert
            sale.TotalDiscount.Should().Be(expectedDiscount);
        }

        [Fact]
        public void AddItem_ShouldMaintainItemOrder()
        {
            // Arrange
            var sale = SaleTestData.GenerateEmptySale();
            var product1 = ProductTestData.GenerateValidProduct();
            var product2 = ProductTestData.GenerateValidProduct();

            // Act
            sale.AddItem(product1, 1);
            sale.AddItem(product2, 2);

            // Assert
            sale.Items[0].Product.Should().Be(product1);
            sale.Items[1].Product.Should().Be(product2);
        }
    }
}
