using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Walks;
using NZWalks.API.Repositories;

namespace NZWalks.API.Tests.Controllers
{
    public class WalksControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IWalkRepository> _walkRepositoryMock;
        private readonly Mock<ILogger<WalksController>> _loggerMock;
        private readonly WalksController _controller;

        public WalksControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _walkRepositoryMock = new Mock<IWalkRepository>();
            _loggerMock = new Mock<ILogger<WalksController>>();
            _controller = new WalksController(_mapperMock.Object, _walkRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllWalks_ReturnsOkResult_WithListOfWalks()
        {
            // Arrange
            var walks = new List<Walk> { new Walk { Id = Guid.NewGuid(), Name = "Coastal Track", Description = "A scenic coastal walk", LengthInKm = 5.5f } };
            var walkDtos = new List<WalkDto> { new WalkDto { Id = walks[0].Id, Name = "Coastal Track", Description = "A scenic coastal walk", LengthInKm = 5.5f } };

            _walkRepositoryMock.Setup(r => r.GetWalkAsync(null, null, null, false, 1, 1000)).ReturnsAsync(walks);
            _mapperMock.Setup(m => m.Map<List<WalkDto>>(walks)).Returns(walkDtos);

            // Act
            var result = await _controller.GetAllWalks(null, null, null, false, 1, 1000);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDtos = okResult.Value.Should().BeAssignableTo<List<WalkDto>>().Subject;
            returnedDtos.Should().ContainSingle();
            returnedDtos[0].Name.Should().Be("Coastal Track");
        }

        [Fact]
        public async Task GetAllWalks_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            // Arrange
            _walkRepositoryMock.Setup(r => r.GetWalkAsync(null, null, null, false, 1, 1000)).ReturnsAsync((List<Walk>?)null);

            // Act
            var result = await _controller.GetAllWalks(null, null, null, false, 1, 1000);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetWalksById_ReturnsNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            _walkRepositoryMock.Setup(r => r.GetWalkByIdAsync(walkId)).ReturnsAsync((Walk?)null);

            // Act
            var result = await _controller.GetWalksById(walkId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetWalksById_ReturnsOkResult_WhenWalkExists()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            var walk = new Walk { Id = walkId, Name = "Rainforest Loop", Description = "A loop through native bush", LengthInKm = 3.2f };
            var walkDto = new WalkDto { Id = walkId, Name = "Rainforest Loop", Description = "A loop through native bush", LengthInKm = 3.2f };

            _walkRepositoryMock.Setup(r => r.GetWalkByIdAsync(walkId)).ReturnsAsync(walk);
            _mapperMock.Setup(m => m.Map<WalkDto>(walk)).Returns(walkDto);

            // Act
            var result = await _controller.GetWalksById(walkId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<WalkDto>().Subject;
            returnedDto.Name.Should().Be("Rainforest Loop");
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedAtAction_WithNewWalk()
        {
            // Arrange
            var requestDto = new AddWalkRequestsDto { Name = "Summit Trail", Description = "A steep summit trail", LengthInKm = 8.0f, DifficultyId = Guid.NewGuid(), RegionId = Guid.NewGuid() };
            var walkModel = new Walk { Name = "Summit Trail", Description = "A steep summit trail", LengthInKm = 8.0f };
            var createdWalk = new Walk { Id = Guid.NewGuid(), Name = "Summit Trail", Description = "A steep summit trail", LengthInKm = 8.0f };
            var walkDto = new WalkDto { Id = createdWalk.Id, Name = "Summit Trail", Description = "A steep summit trail", LengthInKm = 8.0f };

            _mapperMock.Setup(m => m.Map<Walk>(requestDto)).Returns(walkModel);
            _walkRepositoryMock.Setup(r => r.CreateAsync(walkModel)).ReturnsAsync(createdWalk);
            _mapperMock.Setup(m => m.Map<WalkDto>(createdWalk)).Returns(walkDto);

            // Act
            var result = await _controller.CreateAsync(requestDto);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnedDto = createdResult.Value.Should().BeOfType<WalkDto>().Subject;
            returnedDto.Name.Should().Be("Summit Trail");
        }

        [Fact]
        public async Task UpdateWalkById_ReturnsOkResult_WhenWalkExists()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            var updateDto = new UpdateWalkRequestsDto { Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f, DifficultyId = Guid.NewGuid(), RegionId = Guid.NewGuid() };
            var mappedWalk = new Walk { Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f };
            var updatedWalk = new Walk { Id = walkId, Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f };
            var walkDto = new WalkDto { Id = walkId, Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f };

            _mapperMock.Setup(m => m.Map<Walk>(updateDto)).Returns(mappedWalk);
            _walkRepositoryMock.Setup(r => r.UpdateWalkByIdAsync(walkId, mappedWalk)).ReturnsAsync(updatedWalk);
            _mapperMock.Setup(m => m.Map<WalkDto>(updatedWalk)).Returns(walkDto);

            // Act
            var result = await _controller.UpdateWalkById(walkId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<WalkDto>().Subject;
            returnedDto.Name.Should().Be("Updated Trail");
        }

        [Fact]
        public async Task UpdateWalkById_ReturnsNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            var updateDto = new UpdateWalkRequestsDto { Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f, DifficultyId = Guid.NewGuid(), RegionId = Guid.NewGuid() };
            var mappedWalk = new Walk { Name = "Updated Trail", Description = "An updated trail description", LengthInKm = 4.0f };

            _mapperMock.Setup(m => m.Map<Walk>(updateDto)).Returns(mappedWalk);
            _walkRepositoryMock.Setup(r => r.UpdateWalkByIdAsync(walkId, mappedWalk)).ReturnsAsync((Walk?)null);

            // Act
            var result = await _controller.UpdateWalkById(walkId, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteWalksById_ReturnsOkResult_WhenWalkExists()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            var deletedWalk = new Walk { Id = walkId, Name = "Old Trail", Description = "A trail being removed", LengthInKm = 2.0f };
            var walkDto = new WalkDto { Id = walkId, Name = "Old Trail", Description = "A trail being removed", LengthInKm = 2.0f };

            _walkRepositoryMock.Setup(r => r.DeleteWalksById(walkId)).ReturnsAsync(deletedWalk);
            _mapperMock.Setup(m => m.Map<WalkDto>(deletedWalk)).Returns(walkDto);

            // Act
            var result = await _controller.DeleteWalksById(walkId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<WalkDto>().Subject;
            returnedDto.Name.Should().Be("Old Trail");
        }

        [Fact]
        public async Task DeleteWalksById_ReturnsNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            var walkId = Guid.NewGuid();
            _walkRepositoryMock.Setup(r => r.DeleteWalksById(walkId)).ReturnsAsync((Walk?)null);

            // Act
            var result = await _controller.DeleteWalksById(walkId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
