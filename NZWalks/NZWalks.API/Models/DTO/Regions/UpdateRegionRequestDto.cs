using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO.Regions
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Code should be minimun 2 digit long.")]
        [MaxLength(3, ErrorMessage = "Code should be maximum 3 digit long.")]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name should be 100 characters Long")]
        public required string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
