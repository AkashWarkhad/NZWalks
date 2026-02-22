using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Controllers.Helper;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.Controllers
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HeaderVersioningController : ControllerBase
    {
        private readonly IMapper _mapper;

        public HeaderVersioningController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetHeaderVersion1()
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = _mapper.Map<List<CountryDtoV1>>(countryDomainModel);

            return Ok(countryDto);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetHeaderVesrion2()
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = _mapper.Map<List<CountryDtoV2>>(countryDomainModel);

            return Ok(countryDto);
        }
    }
}
