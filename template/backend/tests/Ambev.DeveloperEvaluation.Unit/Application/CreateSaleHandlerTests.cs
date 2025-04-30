using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleValidatorService> _validatorMock;
        private readonly Mock<ISaleFactoryService> _factoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _validatorMock = new Mock<ISaleValidatorService>();
            _factoryMock = new Mock<ISaleFactoryService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _validatorMock.Object,
                _factoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateSale_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateSaleTestData.GenerateValidCommand();
            var saleDto = CreateSaleTestData.GenerateValidSaleDto();
            var sale = CreateSaleTestData.GenerateSampleSale();
            var expectedResult = new CreateSaleResult();

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.CreateAsync(sale, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(sale);

            _mapperMock.Setup(x => x.Map<CreateSaleResult>(sale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResult);

            _mapperMock.Verify(x => x.Map<SaleDto>(command), Times.Once);
            _validatorMock.Verify(x => x.ValidateAsync(saleDto, It.IsAny<CancellationToken>()), Times.Once);
            _factoryMock.Verify(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()), Times.Once);
            _saleRepositoryMock.Verify(x => x.CreateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<CreateSaleResult>(sale), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = CreateSaleTestData.GenerateInvalidCommand();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidCommand, CancellationToken.None));

            // Verify no other methods were called
            _mapperMock.Verify(x => x.Map<SaleDto>(It.IsAny<CreateSaleCommand>()), Times.Never);
            _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<SaleDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _factoryMock.Verify(x => x.CreateSaleAsync(It.IsAny<SaleDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _saleRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldPropagateValidationException_FromValidatorService()
        {
            // Arrange
            var command = CreateSaleTestData.GenerateValidCommand();
            var saleDto = CreateSaleTestData.GenerateValidSaleDto();

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
            var command = CreateSaleTestData.GenerateValidCommand();
            var saleDto = CreateSaleTestData.GenerateValidSaleDto();

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
            var command = CreateSaleTestData.GenerateValidCommand();
            var saleDto = CreateSaleTestData.GenerateValidSaleDto();
            var sale = CreateSaleTestData.GenerateSampleSale();

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.CreateAsync(sale, It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldMapAllPropertiesCorrectly_InFinalResult()
        {
            // Arrange
            var command = CreateSaleTestData.GenerateValidCommand();
            var saleDto = CreateSaleTestData.GenerateValidSaleDto();
            var sale = CreateSaleTestData.GenerateSampleSale();
            var expectedResult = new CreateSaleResult
            {
                Id = sale.Id,
                SaleNumber = 12345,
                Date = sale.Date,
                CustomerId = sale.Customer.Id,
                BranchId = sale.Branch.Id,
                Items = sale.Items,
                Status = sale.Status,
                TotalAmount = sale.TotalAmount,
                TotalDiscount = sale.TotalDiscount,
                FinalAmount = sale.FinalAmount
            };

            _mapperMock.Setup(x => x.Map<SaleDto>(command))
                      .Returns(saleDto);

            _factoryMock.Setup(x => x.CreateSaleAsync(saleDto, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(sale);

            _saleRepositoryMock.Setup(x => x.CreateAsync(sale, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(sale);

            _mapperMock.Setup(x => x.Map<CreateSaleResult>(sale))
                      .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            result.SaleNumber.Should().Be(12345);
            result.Date.Should().Be(sale.Date);
            result.CustomerId.Should().Be(sale.Customer.Id);
            result.BranchId.Should().Be(sale.Branch.Id);
            result.Items.Should().BeEquivalentTo(sale.Items);
            result.Status.Should().Be(sale.Status);
            result.TotalAmount.Should().Be(sale.TotalAmount);
            result.TotalDiscount.Should().Be(sale.TotalDiscount);
            result.FinalAmount.Should().Be(sale.FinalAmount);
        }
    }
}
