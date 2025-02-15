using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.Walks;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        private readonly Logger<WalksController> logger;

        public WalksController(IMapper mapper, IWalkRepository walkRepository, Logger<WalksController> logger)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
            this.logger = logger;
        }

        // --------------------------Get All Method -------------------------------------------
        // GET: https://localhost:7285/api/Walks?filterOn=name&filterQuery=walks&sortBy=name&isAscending=true&pageNumber=1&pageSize=10

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllWalks(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepository.GetWalkAsync(
                filterOn, 
                filterQuery, 
                sortBy, 
                isAscending,
                pageNumber,
                pageSize);

            if (walksDomainModel == null)
            {
                logger.LogWarning("Sorry to inconvenience record not found !!!");
                return NotFound();
            }

            // Convert Walks Domain Model into DTO
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomainModel);

            return Ok(walksDto);
        }

        // ----------------------- Get Id Method --------------------------------------------------
        // GET: https://localhost/7265/api/Walks/id

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetWalksById([FromRoute] Guid id)
        {
            // Fetch the Data from the DB
            var walksDomain = await _walkRepository.GetWalkByIdAsync(id);

            if (walksDomain == null)
            {
                return NotFound();
            }

            // Convert Domain into DTO
            var walksDto = _mapper.Map<WalkDto>(walksDomain);

            return Ok(walksDto);
        }

        // -------------------------- Post Method -------------------------------------------
        // Create a Walks 
        // POST : https://localhost:7285/api/Walks

        [HttpPost]
        [ValidateModelAttribute]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestsDto walkRequestsDto)
        {

            // Convert DTO to Domain Models
            var walkDomainModels = _mapper.Map<Walk>(walkRequestsDto);

            // Add walkDomainModels into Database
            try
            {
                walkDomainModels = await _walkRepository.CreateAsync(walkDomainModels);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorDetailsDto
                {
                    ErrorCode = "Server Error",
                    ErrorMessage = ex.Message + " " + ex.InnerException?.Message
                });
            }

            // Convert Domain model into Dto
            var walkDto = _mapper.Map<WalkDto>(walkDomainModels);

            return CreatedAtAction(nameof(GetWalksById), new { id = walkDto.Id }, walkDto);
        }

        // ----------------------- Put Update Method -----------------------------------------------
        // PUT: https://localhost:7265/api/Walks/id

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateWalkById([FromRoute] Guid id, [FromBody] UpdateWalkRequestsDto updateWalks)
        {
            // Convert Dto to Domain models
            var walkDomainModel = _mapper.Map<Walk>(updateWalks);

            // Fetch the record & update the Walks data in the database
            var updatedWalk = await _walkRepository.UpdateWalkByIdAsync(id, walkDomainModel);

            if (updatedWalk == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO models
            return Ok(_mapper.Map<WalkDto>(updatedWalk));
        }

        //  ----------------------- Delete Method By Id ----------------------------------------------------
        // DELETE: https://localhost:7265/api/Walks/id

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteWalksById([FromRoute] Guid id)
        {
            // Delete the walks from the Database
            var deltedWalkDomain = await _walkRepository.DeleteWalksById(id);

            if (deltedWalkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain Models to Dto
            var deltedWalksDTO = _mapper.Map<WalkDto>(deltedWalkDomain);

            return Ok(deltedWalksDTO);
        }
    }
}
