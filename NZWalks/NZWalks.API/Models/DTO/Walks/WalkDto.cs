using NZWalks.API.Models.DTO.Difficulty;
using NZWalks.API.Models.DTO.Regions;

namespace NZWalks.API.Models.DTO.Walks
{
    public class WalkDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public float LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public DifficultyDto? Difficulty { get; set; }

        public RegionDto? Region { get; set; }
    }
}
