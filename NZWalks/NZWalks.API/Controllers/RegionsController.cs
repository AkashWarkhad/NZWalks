using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.Regions;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // ----------------------------------------GET All Regions --------------------------------------------------------
        // GET: https://localhost:1234/api/Regions
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<ActionResult> GetAll()
        {
            _logger.LogInformation("Information :GetAll Action Method was invoked..");
            _logger.LogWarning("Warning : GetAll Action Method was invoked..");
            _logger.LogDebug("Debug :GetAll Action Method was invoked..");
            _logger.LogError("Error : GetAll Action Method was invoked..");

            // Get the data from database in the domain models.
            var regionsDomain = await _regionRepository.GetAllAsync();

            _logger.LogInformation($"Successfully retrieved Regions data from the database : {JsonSerializer.Serialize(regionsDomain)}");

            // Map domain models into Dto models & return Dto
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //---------------------------------------- Get Region By Id ------------------------------------------------------------
        // Improve error handling
        // GET: https://localhost:1234/api/Regions/Id

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get RegionDomain data based on Id from the Database
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map RegionDomain models to RegionDto models & return result
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        //----------------------- POST METHODS -----------------------------
        // Post: https://localhost:1223/api/regions
        
        [HttpPost]
        [ValidateModelAttribute]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto requestDto)
        {
            // Map Dto models to Domain models
            var regionModel = _mapper.Map<Region>(requestDto);

            try
            {
                // Use domain model to create record in Database
                regionModel = await _regionRepository.CreateAsync(regionModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDetailsDto()
                {
                    ErrorCode = "Internal_Server_Error",
                    ErrorMessage = ex.Message + Environment.NewLine + ex.InnerException?.Message
                });
            }

            // Convert domain model into Dto models
            var regionDto = _mapper.Map<RegionDto>(regionModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
          
        }

        //----------------------- PUT METHOD ---------------------------
        // PUT: https://localhost:1223/api/regions/id

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttribute]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRequestDto)
        {
            
            // Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRequestDto);

            // Find the Regions in the Database & modify it & save in the Database
            try
            {
                regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message} {ex.InnerException?.Message}");
            }

            // Convert Domain model to DTO & return result
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        //----------------------- DELETE METHOD ----------------------
        // DELETE https://localhost:1234/api/regions/id

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Find the region in the database
            var regionDomain = await _regionRepository.DeleteAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Convert Domain model to Dto Model & return Result
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }
    }
}
