using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.Controllers
{
    [Route("v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesUriVersioningController : ControllerBase
    {
        private readonly IMapper mapper;

        public CountriesUriVersioningController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // --------------------------- Version V1 with Country -------------------------------------
        [HttpGet("GetVersion1")]
        [MapToApiVersion("1.0")]
        public ActionResult<List<CountryDtoV1>> GetVersion1()
        {
            var countryDomainModel = CountryDomainModelsData;

            // Convert Domain model into Dto
            var countryDto = mapper.Map<List<CountryDtoV1>>(countryDomainModel);

            return Ok(countryDto);
        }

        // -------------------------------------- Version V2 with Country name ---------------------------------------
        [HttpGet("GetVersion2")]
        [MapToApiVersion("2.0")]
        public ActionResult<List<CountryDtoV2>> GetVersion2()
        {
            var countryDomainModel = CountryDomainModelsData;

            // Convert Domain model into Dto
            var countryDto = mapper.Map<List<CountryDtoV2>>(countryDomainModel);
            
            return Ok(countryDto);
        }

        private List<Country> CountryDomainModelsData => 
            [
                new Country()
                {
                    ID = 1,
                    Name = "India"
                },
                new Country()
                {
                    ID = 2,
                    Name = "USA"
                },
                new Country()
                {
                    ID = 3,
                    Name = "Indonesia"
                },
                new Country()
                {
                    ID = 4,
                    Name = "France"
                },
                new Country()
                {
                    ID = 5,
                    Name = "Russia"
                }
            ];
    }
}
