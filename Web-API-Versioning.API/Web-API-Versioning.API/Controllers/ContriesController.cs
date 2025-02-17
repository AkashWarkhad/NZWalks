using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ContriesController : ControllerBase
    {
        // --------------------------- Version V1 with Country -------------------------------------
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {
            var countryDomainModel = GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = new List<CountryDtoV1>();

            foreach (var country in countryDomainModel)
            {
                countryDto.Add(new CountryDtoV1()
                {
                    ID = country.ID,
                    Name = country.Name
                });
            }

            return Ok(countryDto);
        }

        // -------------------------------------- Version V2 with Country name ---------------------------------------
        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetV2()
        {
            var countryDomainModel = GetCountryDomainModelsData();

            // Convert Domain model into Dto
            var countryDto = new List<CountryDtoV2>();

            foreach (var country in countryDomainModel)
            {
                countryDto.Add(new CountryDtoV2()
                {
                    ID = country.ID,
                    CountryName = country.Name
                });
            }

            return Ok(countryDto);
        }

        private List<Country> GetCountryDomainModelsData()
        {
            return new List<Country>
            {
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
                },
                new Country()
                {
                    ID = 6,
                    Name = "UK"
                },
                new Country()
                {
                    ID = 7,
                    Name = "Iteli"
                },
                new Country()
                {
                    ID = 8,
                    Name = "Shrilanka"
                },
            };
        }
    }
}
