using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO.Regions
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Code value should be minimum 2 digit long")]
        [MaxLength(3, ErrorMessage = "Code value should be maximum 3 digit long")]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name should not be more than 100 letters long")]
        public required string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
