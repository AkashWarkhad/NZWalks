using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Regions;
using NZWalks.API.Repositories;
using Xunit;

namespace NZWalks.API.Tests.Controllers
{
    public class RegionsControllerTests
    {
        private readonly Mock<IRegionRepository> _regionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RegionsController>> _loggerMock;
        private readonly RegionsController _controller;

        public RegionsControllerTests()
        {
            _regionRepositoryMock = new Mock<IRegionRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RegionsController>>();
            _controller = new RegionsController(_regionRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfRegions()
        {
            // Arrange
            var regions = new List<Region> { new Region { Id = Guid.NewGuid(), Code = "AKL", Name = "Auckland" } };
            var regionDtos = new List<RegionDto> { new RegionDto { Id = regions[0].Id, Code = "AKL", Name = "Auckland" } };

            _regionRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(regions);
            _mapperMock.Setup(m => m.Map<List<RegionDto>>(regions)).Returns(regionDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDtos = okResult.Value.Should().BeAssignableTo<List<RegionDto>>().Subject;
            returnedDtos.Should().ContainSingle();
            returnedDtos[0].Name.Should().Be("Auckland");
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            _regionRepositoryMock.Setup(r => r.GetByIdAsync(regionId)).ReturnsAsync((Region?)null);

            // Act
            var result = await _controller.GetById(regionId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenRegionExists()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var region = new Region { Id = regionId, Code = "WLG", Name = "Wellington" };
            var regionDto = new RegionDto { Id = regionId, Code = "WLG", Name = "Wellington" };

            _regionRepositoryMock.Setup(r => r.GetByIdAsync(regionId)).ReturnsAsync(region);
            _mapperMock.Setup(m => m.Map<RegionDto>(region)).Returns(regionDto);

            // Act
            var result = await _controller.GetById(regionId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<RegionDto>().Subject;
            returnedDto.Name.Should().Be("Wellington");
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithNewRegion()
        {
            // Arrange
            var requestDto = new AddRegionRequestDto { Code = "OTA", Name = "Otago" };
            var regionModel = new Region { Code = "OTA", Name = "Otago" };
            var createdRegion = new Region { Id = Guid.NewGuid(), Code = "OTA", Name = "Otago" };
            var regionDto = new RegionDto { Id = createdRegion.Id, Code = "OTA", Name = "Otago" };

            _mapperMock.Setup(m => m.Map<Region>(requestDto)).Returns(regionModel);
            _regionRepositoryMock.Setup(r => r.CreateAsync(regionModel)).ReturnsAsync(createdRegion);
            _mapperMock.Setup(m => m.Map<RegionDto>(createdRegion)).Returns(regionDto);

            // Act
            var result = await _controller.Create(requestDto);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnedDto = createdResult.Value.Should().BeOfType<RegionDto>().Subject;
            returnedDto.Name.Should().Be("Otago");
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenRegionExists()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var updateRequestDto = new UpdateRegionRequestDto { Code = "CAN", Name = "Canterbury" };
            var mappedRegion = new Region { Code = "CAN", Name = "Canterbury" };
            var updatedRegion = new Region { Id = regionId, Code = "CAN", Name = "Canterbury" };
            var regionDto = new RegionDto { Id = regionId, Code = "CAN", Name = "Canterbury" };

            _mapperMock.Setup(m => m.Map<Region>(updateRequestDto)).Returns(mappedRegion);
            _regionRepositoryMock.Setup(r => r.UpdateAsync(regionId, mappedRegion)).ReturnsAsync(updatedRegion);
            _mapperMock.Setup(m => m.Map<RegionDto>(updatedRegion)).Returns(regionDto);

            // Act
            var result = await _controller.Update(regionId, updateRequestDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<RegionDto>().Subject;
            returnedDto.Name.Should().Be("Canterbury");
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var updateRequestDto = new UpdateRegionRequestDto { Code = "CAN", Name = "Canterbury" };
            var mappedRegion = new Region { Code = "CAN", Name = "Canterbury" };

            _mapperMock.Setup(m => m.Map<Region>(updateRequestDto)).Returns(mappedRegion);
            _regionRepositoryMock.Setup(r => r.UpdateAsync(regionId, mappedRegion)).ReturnsAsync((Region?)null);

            // Act
            var result = await _controller.Update(regionId, updateRequestDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenRegionExists()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var deletedRegion = new Region { Id = regionId, Code = "MAN", Name = "Manawatu" };
            var regionDto = new RegionDto { Id = regionId, Code = "MAN", Name = "Manawatu" };

            _regionRepositoryMock.Setup(r => r.DeleteAsync(regionId)).ReturnsAsync(deletedRegion);
            _mapperMock.Setup(m => m.Map<RegionDto>(deletedRegion)).Returns(regionDto);

            // Act
            var result = await _controller.Delete(regionId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDto = okResult.Value.Should().BeOfType<RegionDto>().Subject;
            returnedDto.Name.Should().Be("Manawatu");
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            _regionRepositoryMock.Setup(r => r.DeleteAsync(regionId)).ReturnsAsync((Region?)null);

            // Act
            var result = await _controller.Delete(regionId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
