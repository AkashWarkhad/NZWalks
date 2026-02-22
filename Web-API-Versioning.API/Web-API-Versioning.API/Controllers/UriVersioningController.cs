using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Controllers.Helper;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.Controllers
{
    [Route("v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UriVersioningController : ControllerBase
    {
        private readonly IMapper mapper;

        public UriVersioningController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // --------------------------- Version V1 with Country -------------------------------------
        [HttpGet("UriVersioning")]
        [MapToApiVersion("1.0")]
        public ActionResult<List<CountryDtoV1>> GetVersion1()
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = mapper.Map<List<CountryDtoV1>>(countryDomainModel);

            return Ok(countryDto);
        }

        // -------------------------------------- Version V2 with Country name ---------------------------------------
        [HttpGet("UriVersioning")]
        [MapToApiVersion("2.0")]
        public ActionResult<List<CountryDtoV2>> GetVersion2()
        {
            var countryDomainModel = ControllerHelper.GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = mapper.Map<List<CountryDtoV2>>(countryDomainModel);

            return Ok(countryDto);
        }
    }
}
