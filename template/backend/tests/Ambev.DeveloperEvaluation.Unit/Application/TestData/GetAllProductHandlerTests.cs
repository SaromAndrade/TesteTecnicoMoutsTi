using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public class GetAllProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductHandler _handler;
        public GetAllProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllProductHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnPaginatedProducts_WhenQueryIsValid()
        {
            // Arrange
            var products = ProductTestData.GenerateProducts(15);
            var query = ProductTestData.GenerateValidQuery(page: 1, size: 10);
            var expectedTotalItems = 15;
            var expectedPaginatedProducts = products.Take(10).ToList();

            _productRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((expectedPaginatedProducts, expectedTotalItems));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expectedPaginatedProducts);
            result.TotalItems.Should().Be(expectedTotalItems);

            _productRepositoryMock.Verify(x => x.GetAllAsync(
                query.Page,
                query.Size,
                query.Order,
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenQueryIsInvalid()
        {
            // Arrange
            var invalidQuery = ProductTestData.GenerateInvalidQuery();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidQuery, CancellationToken.None));

            _productRepositoryMock.Verify(x => x.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ShouldApplyOrdering_WhenOrderParameterIsProvided()
        {
            // Arrange
            var products = ProductTestData.GenerateProducts(5);
            var query = ProductTestData.GenerateValidQuery(order: "price desc");
            var expectedTotalItems = 5;

            _productRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((products, expectedTotalItems));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _productRepositoryMock.Verify(x => x.GetAllAsync(
                query.Page,
                query.Size,
                query.Order,
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var emptyProducts = new List<Product>();
            var query = ProductTestData.GenerateValidQuery();
            var expectedTotalItems = 0;

            _productRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((emptyProducts, expectedTotalItems));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }
        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var query = ProductTestData.GenerateValidQuery();

            _productRepositoryMock.Setup(x => x.GetAllAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
        [Theory]
        [InlineData(1, 10, "name asc")]
        [InlineData(2, 5, "price desc")]
        [InlineData(3, 20, "name")]
        public async Task Handle_ShouldWorkWithDifferentPaginationParameters(int page, int size, string order)
        {
            // Arrange
            var products = ProductTestData.GenerateProducts(50);
            var query = new GetAllProductQuery { Page = page, Size = size, Order = order };
            var expectedPaginatedProducts = products.Skip((page - 1) * size).Take(size).ToList();

            _productRepositoryMock.Setup(x => x.GetAllAsync(
                    page,
                    size,
                    order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((expectedPaginatedProducts, products.Count));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expectedPaginatedProducts);
            result.TotalItems.Should().Be(products.Count);
        }
    }
}
