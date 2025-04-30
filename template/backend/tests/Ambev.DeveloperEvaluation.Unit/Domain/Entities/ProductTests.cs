using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Product_WithValidData_ShouldBeValid()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(GetPropertyValue(product, "Name")));
            Assert.False(string.IsNullOrWhiteSpace(GetPropertyValue(product, "Description")));
            Assert.InRange(product.Price, 1, 1000);
            Assert.NotNull(product.Rating);
            Assert.InRange(product.Rating!.Rate, 0, 5);
            Assert.InRange(product.Rating.Count, 0, 1000);
        }
        private static string GetPropertyValue(Product product, string propertyName)
        {
            return product.GetType().GetProperty(propertyName)?.GetValue(product)?.ToString() ?? string.Empty;
        }
        [Fact]
        public void Product_WithNullRating_ShouldBeValid()
        {
            // Arrange
            var product = ProductTestData.GenerateProductWithoutRating();

            // Assert
            Assert.NotNull(product);
            Assert.Null(product.Rating);
        }
        [Fact]
        public void Product_WithInvalidPrice_ShouldHaveNegativePrice()
        {
            // Arrange
            var product = ProductTestData.GenerateProductWithInvalidPrice();

            // Assert
            Assert.True(product.Price < 0);
        }
        [Fact]
        public void Product_WithEmptyName_ShouldHaveEmptyName()
        {
            // Arrange
            var product = ProductTestData.GenerateProductWithEmptyName();

            // Assert
            var name = GetPropertyValue(product, "Name");
            Assert.True(string.IsNullOrWhiteSpace(name));
        }
        [Fact]
        public void Rating_WithInvalidValues_ShouldHaveOutOfRangeRate()
        {
            // Arrange
            var rating = ProductTestData.GenerateInvalidRating();

            // Assert
            Assert.True(rating.Rate > 5);
            Assert.True(rating.Count < 0);
        }

    }
}
