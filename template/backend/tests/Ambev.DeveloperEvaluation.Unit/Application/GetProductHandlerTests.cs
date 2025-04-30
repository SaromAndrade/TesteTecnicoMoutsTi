using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductHandler _handler;

        public GetProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenIdIsValidAndProductExists()
        {
            // Arrange
            var query = GetProductTestData.GenerateValidQuery();
            var product = GetProductTestData.GenerateSampleProduct();
            var expectedResult = new GetProductResult(); // O mapper vai preencher isso

            _productRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(product);

            _mapperMock.Setup(x => x.Map<GetProductResult>(product))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResult);
            _productRepositoryMock.Verify(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetProductResult>(product), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenIdIsInvalid()
        {
            // Arrange
            var invalidQuery = GetProductTestData.GenerateInvalidQuery();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidQuery, CancellationToken.None));

            _productRepositoryMock.Verify(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullMappedResult_WhenProductDoesNotExist()
        {
            // Arrange
            var query = GetProductTestData.GenerateValidQuery();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Product)null);

            _mapperMock.Setup(x => x.Map<GetProductResult>(null))
                      .Returns((GetProductResult)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _productRepositoryMock.Verify(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetProductResult>(null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var query = GetProductTestData.GenerateValidQuery();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCorrectlyMapAllProperties_WhenProductExists()
        {
            // Arrange
            var query = GetProductTestData.GenerateValidQuery();
            var product = GetProductTestData.GenerateSampleProduct();
            var expectedResult = new GetProductResult
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Rating = product.Rating
            };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(product);

            _mapperMock.Setup(x => x.Map<GetProductResult>(product))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
            result.Name.Should().Be(product.Name);
            result.Description.Should().Be(product.Description);
            result.Price.Should().Be(product.Price);
            result.Rating.Should().BeEquivalentTo(product.Rating);
        }
    }
}
