using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
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
    public class GetSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnSale_WhenSaleExists()
        {
            // Arrange
            var query = GetSaleTestData.GenerateValidQuery();
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var expectedResult = GetSaleTestData.GenerateExpectedResult(sale);

            _saleRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(sale);

            _mapperMock.Setup(x => x.Map<GetSaleResult>(sale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
            _saleRepositoryMock.Verify(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetSaleResult>(sale), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenSaleDoesNotExist()
        {
            // Arrange
            var query = GetSaleTestData.GenerateValidQuery();

            _saleRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Sale)null);

            _mapperMock.Setup(x => x.Map<GetSaleResult>(null))
                      .Returns((GetSaleResult)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _saleRepositoryMock.Verify(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetSaleResult>(null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var query = GetSaleTestData.GenerateValidQuery();

            _saleRepositoryMock.Setup(x => x.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCorrectlyMapAllProperties_WhenSaleExists()
        {
            // Arrange
            var query = GetSaleTestData.GenerateValidQuery();
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var expectedResult = GetSaleTestData.GenerateExpectedResult(sale);

            _saleRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(sale);

            _mapperMock.Setup(x => x.Map<GetSaleResult>(sale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            result.SaleNumber.Should().Be(sale.SaleNumber ?? 0);
            result.Date.Should().Be(sale.Date);
            result.CustomerId.Should().Be(sale.Customer.Id);
            result.BranchId.Should().Be(sale.Branch.Id);
            result.Items.Should().BeEquivalentTo(sale.Items);
            result.Status.Should().Be(sale.Status);
            result.TotalAmount.Should().Be(sale.TotalAmount);
            result.TotalDiscount.Should().Be(sale.TotalDiscount);
            result.FinalAmount.Should().Be(sale.FinalAmount);
        }

        [Fact]
        public async Task Handle_ShouldWorkWithDifferentSaleTypes()
        {
            // Arrange
            var query = GetSaleTestData.GenerateValidQuery();

            // Testar com diferentes tipos de venda
            var testCases = new[]
            {
            SaleTestData.GenerateEmptySale(),
            SaleTestData.GenerateSaleWithSingleItem(),
            SaleTestData.GenerateSaleWithMultipleItems(),
            SaleTestData.GenerateCancelledSale()
        };

            foreach (var sale in testCases)
            {
                var expectedResult = GetSaleTestData.GenerateExpectedResult(sale);

                _saleRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(sale);

                _mapperMock.Setup(x => x.Map<GetSaleResult>(sale))
                          .Returns(expectedResult);

                // Act
                var result = await _handler.Handle(query, CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(sale.Id);
                result.Items.Count.Should().Be(sale.Items.Count);
                result.Status.Should().Be(sale.Status);
            }
        }
    }
}
