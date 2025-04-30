using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetAllSalesHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllSalesHandler _handler;
        public GetAllSalesHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllSalesHandler(_saleRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnPaginatedSales_WhenQueryIsValid()
        {
            // Arrange
            var query = GetAllSalesTestData.GenerateValidQuery(page: 1, size: 10);
            var sales = GetAllSalesTestData.GenerateSalesList(15);
            var expectedPaginatedSales = sales.Take(10).ToList();
            var expectedTotalItems = 15;

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((expectedPaginatedSales, expectedTotalItems));

            _mapperMock.Setup(x => x.Map<List<Sale>>(expectedPaginatedSales))
                     .Returns(expectedPaginatedSales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sales.Should().BeEquivalentTo(expectedPaginatedSales);
            result.TotalItems.Should().Be(expectedTotalItems);

            _saleRepositoryMock.Verify(x => x.GetAllAsync(
                query.Page,
                query.Size,
                query.Order,
                It.IsAny<CancellationToken>()), Times.Once);

            _mapperMock.Verify(x => x.Map<List<Sale>>(expectedPaginatedSales), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldApplyOrdering_WhenOrderParameterIsProvided()
        {
            // Arrange
            var query = GetAllSalesTestData.GenerateValidQuery(order: "date desc");
            var sales = GetAllSalesTestData.GenerateSalesList(5);
            var expectedTotalItems = 5;

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((sales, expectedTotalItems));

            _mapperMock.Setup(x => x.Map<List<Sale>>(sales))
                     .Returns(sales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _saleRepositoryMock.Verify(x => x.GetAllAsync(
                query.Page,
                query.Size,
                query.Order,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoSalesExist()
        {
            // Arrange
            var query = GetAllSalesTestData.GenerateValidQuery();
            var emptySales = new List<Sale>();
            var expectedTotalItems = 0;

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((emptySales, expectedTotalItems));

            _mapperMock.Setup(x => x.Map<List<Sale>>(emptySales))
                     .Returns(emptySales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sales.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var query = GetAllSalesTestData.GenerateValidQuery();

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
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
        [InlineData(1, 10, "")]
        [InlineData(2, 5, "date desc")]
        [InlineData(3, 20, "totalAmount asc")]
        public async Task Handle_ShouldWorkWithDifferentPaginationParameters(int page, int size, string order)
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = page, Size = size, Order = order };
            var sales = GetAllSalesTestData.GenerateSalesList(50);
            var expectedPaginatedSales = sales.Skip((page - 1) * size).Take(size).ToList();

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
                    page,
                    size,
                    order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((expectedPaginatedSales, sales.Count));

            _mapperMock.Setup(x => x.Map<List<Sale>>(expectedPaginatedSales))
                     .Returns(expectedPaginatedSales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sales.Should().BeEquivalentTo(expectedPaginatedSales);
            result.TotalItems.Should().Be(sales.Count);
        }

        [Fact]
        public async Task Handle_ShouldCorrectlyMapAllProperties_InResultItems()
        {
            // Arrange
            var query = GetAllSalesTestData.GenerateValidQuery();
            var sales = GetAllSalesTestData.GenerateSalesList(1);
            var expectedTotalItems = 1;

            _saleRepositoryMock.Setup(x => x.GetAllAsync(
                    query.Page,
                    query.Size,
                    query.Order,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((sales, expectedTotalItems));

            _mapperMock.Setup(x => x.Map<List<Sale>>(sales))
                     .Returns(sales);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sales.Should().ContainSingle();

            var saleResult = result.Sales[0];
            var originalSale = sales[0];

            saleResult.Id.Should().Be(originalSale.Id);
            saleResult.SaleNumber.Should().Be(originalSale.SaleNumber);
            saleResult.Date.Should().Be(originalSale.Date);
            saleResult.Customer.Should().Be(originalSale.Customer);
            saleResult.Branch.Should().Be(originalSale.Branch);
            saleResult.Items.Should().BeEquivalentTo(originalSale.Items);
            saleResult.Status.Should().Be(originalSale.Status);
            saleResult.TotalAmount.Should().Be(originalSale.TotalAmount);
            saleResult.TotalDiscount.Should().Be(originalSale.TotalDiscount);
            saleResult.FinalAmount.Should().Be(originalSale.FinalAmount);
        }
    }
}
