using Ambev.DeveloperEvaluation.Application.Branchs.GetAllBranchs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
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
    public class GetAllBranchsHandlerTests
    {
        private readonly Mock<IBranchRepository> _branchRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllBranchsHandler _handler;

        public GetAllBranchsHandlerTests()
        {
            _branchRepositoryMock = new Mock<IBranchRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllBranchsHandler(_branchRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllBranches_WhenRepositoryReturnsData()
        {
            // Arrange
            var branches = new List<Branch>
            {
                new Branch { Id = Guid.NewGuid(), Name = "Branch 1", Location = "Location 1" },
                new Branch { Id = Guid.NewGuid(), Name = "Branch 2", Location = "Location 2" }
            };

            var expectedResult = new GetAllBranchsResult { Data = branches };

            _branchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                                .ReturnsAsync(branches);

            var query = new GetAllBranchsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(branches);
            _branchRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenRepositoryReturnsNoData()
        {
            // Arrange
            var emptyBranchList = new List<Branch>();

            _branchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                                .ReturnsAsync(emptyBranchList);

            var query = new GetAllBranchsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            _branchRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            _branchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new Exception("Database error"));

            var query = new GetAllBranchsQuery();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
            _branchRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
