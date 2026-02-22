using AutoMapper;
using Web_API_Versioning.API.Models.Domain;
using Web_API_Versioning.API.Models.DTO;

namespace Web_API_Versioning.API.MapperProfiles
{
    public class CountriesMappingProfile : Profile
    {
        public CountriesMappingProfile()
        {
            CreateMap<Country, CountryDtoV1>();

            CreateMap<Country, CountryDtoV2>()
                .ForMember(x => x.CountryName, opt => opt.MapFrom(y => y.Name));
        }
    }
}
