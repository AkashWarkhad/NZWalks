using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CountriesQueryBasedVersioningController : ControllerBase
    {
        private readonly IMapper _mapper;

        public CountriesQueryBasedVersioningController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetQueryBasedVersion1() 
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto & Return
            return Ok(_mapper.Map<List<CountryDtoV1>>(countryDomainModel));
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetQueryBasedVersion2()
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto & Return
            return Ok(_mapper.Map<List<CountryDtoV2>>(countryDomainModel));
        }
    }
}
