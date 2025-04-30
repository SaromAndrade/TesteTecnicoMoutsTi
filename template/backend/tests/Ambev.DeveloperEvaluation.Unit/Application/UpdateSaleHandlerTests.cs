using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleValidatorService> _validatorMock;
        private readonly Mock<ISaleFactoryService> _factoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _validatorMock = new Mock<ISaleValidatorService>();
            _factoryMock = new Mock<ISaleFactoryService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateSaleHandler(
                _saleRepositoryMock.Object,
                _validatorMock.Object,
                _factoryMock.Object,
                _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldUpdateSale_WhenCommandIsValid()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            var saleDto = UpdateSaleTestData.GenerateValidSaleDto();
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var updatedSale = SaleTestData.GenerateSaleWithSingleItem();
            var expectedResult = UpdateSaleTestData.GenerateExpectedResult(updatedSale);

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.UpdateAsync(command.Id, sale, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(updatedSale);

            _mapperMock.Setup(x => x.Map<UpdateSaleResult>(updatedSale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

            _mapperMock.Verify(x => x.Map<SaleDto>(command), Times.Once);
            _validatorMock.Verify(x => x.ValidateAsync(saleDto, It.IsAny<CancellationToken>()), Times.Once);
            _factoryMock.Verify(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()), Times.Once);
            _saleRepositoryMock.Verify(x => x.UpdateAsync(command.Id, sale, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UpdateSaleResult>(updatedSale), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = UpdateSaleTestData.GenerateInvalidCommand();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidCommand, CancellationToken.None));

            // Verify no other methods were called
            _mapperMock.Verify(x => x.Map<SaleDto>(It.IsAny<UpdateSaleCommand>()), Times.Never);
            _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<SaleDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _factoryMock.Verify(x => x.CreateSaleAsync(It.IsAny<SaleDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _saleRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldPropagateValidationException_FromValidatorService()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            var saleDto = UpdateSaleTestData.GenerateValidSaleDto();

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _validatorMock.Setup(x => x.ValidateAsync(saleDto, It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new DomainException("Validation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_FromFactoryService()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            var saleDto = UpdateSaleTestData.GenerateValidSaleDto();

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Factory error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_FromRepository()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            var saleDto = UpdateSaleTestData.GenerateValidSaleDto();
            var sale = SaleTestData.GenerateSaleWithMultipleItems();

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.UpdateAsync(command.Id, sale, It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCorrectlyMapAllProperties_InFinalResult()
        {
            // Arrange
            var command = UpdateSaleTestData.GenerateValidCommand();
            var saleDto = UpdateSaleTestData.GenerateValidSaleDto();
            var sale = SaleTestData.GenerateSaleWithMultipleItems();
            var updatedSale = SaleTestData.GenerateSaleWithSingleItem();
            var expectedResult = UpdateSaleTestData.GenerateExpectedResult(updatedSale);

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.UpdateAsync(command.Id, sale, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(updatedSale);

            _mapperMock.Setup(x => x.Map<UpdateSaleResult>(updatedSale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(updatedSale.Id);
            result.SaleNumber.Should().Be(updatedSale.SaleNumber ?? 0);
            result.Date.Should().Be(updatedSale.Date);
            result.CustomerId.Should().Be(updatedSale.Customer.Id);
            result.BranchId.Should().Be(updatedSale.Branch.Id);
            result.Items.Should().BeEquivalentTo(updatedSale.Items);
            result.Status.Should().Be(updatedSale.Status);
            result.TotalAmount.Should().Be(updatedSale.TotalAmount);
            result.TotalDiscount.Should().Be(updatedSale.TotalDiscount);
            result.FinalAmount.Should().Be(updatedSale.FinalAmount);
        }
    }
}
