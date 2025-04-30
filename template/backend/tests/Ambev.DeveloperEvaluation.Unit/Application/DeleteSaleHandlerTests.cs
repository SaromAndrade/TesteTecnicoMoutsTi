using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _handler = new DeleteSaleHandler(_saleRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnSuccessMessage_WhenSaleExists()
        {
            // Arrange
            var command = DeleteSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("Sale deleted successfully");
            _saleRepositoryMock.Verify(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = DeleteSaleTestData.GenerateInvalidCommand();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidCommand, CancellationToken.None));

            _saleRepositoryMock.Verify(x => x.DeleteAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
        {
            // Arrange  
            var command = DeleteSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(false);

            // Act  
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert  
            exception.Message.Should().Be("Product not found");
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var command = DeleteSaleTestData.GenerateValidCommand();
            _saleRepositoryMock.Setup(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var command = new DeleteSaleCommand { Id = expectedId };
            _saleRepositoryMock.Setup(x => x.DeleteAsync(expectedId, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("Sale deleted successfully");
            _saleRepositoryMock.Verify(x => x.DeleteAsync(expectedId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
