using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO.Walks
{
    public class AddWalkRequestsDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name should not be greater than 100 characters.")]
        public required string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description should not be greater than 100 characters.")]
        public required string Description { get; set; }

        [Required]
        [Range(1, 50)]
        public float LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
