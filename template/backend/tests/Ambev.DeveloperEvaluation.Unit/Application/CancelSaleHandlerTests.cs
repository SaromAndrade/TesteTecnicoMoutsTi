using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly CancelSaleHandler _handler;

        public CancelSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _handler = new CancelSaleHandler(_saleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessMessage_WhenSaleExistsAndIsNotCancelled()
        {
            // Arrange
            var command = CancelSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.CancelAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("Sale canceled successfully");
            _saleRepositoryMock.Verify(x => x.CancelAsync(command.SaleNumber, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = CancelSaleTestData.GenerateInvalidCommand();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidCommand, CancellationToken.None));

            _saleRepositoryMock.Verify(x => x.CancelAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
        {
            // Arrange  
            var command = CancelSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.CancelAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(false);

            // Act & Assert  
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            exception.Message.Should().Be("Sale not found");
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var command = CancelSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.CancelAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(9999)]
        [InlineData(123456)]
        public async Task Handle_ShouldWorkWithDifferentValidSaleNumbers(int saleNumber)
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = saleNumber };
            _saleRepositoryMock.Setup(x => x.CancelAsync(saleNumber, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("Sale canceled successfully");
            _saleRepositoryMock.Verify(x => x.CancelAsync(saleNumber, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
