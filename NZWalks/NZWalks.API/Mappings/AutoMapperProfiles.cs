using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Difficulty;
using NZWalks.API.Models.DTO.Regions;
using NZWalks.API.Models.DTO.Walks;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegionDto, Region>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>();
            CreateMap<AddWalkRequestsDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestsDto, Walk>();
        }
    }
}
