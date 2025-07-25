﻿namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public float LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Difficulty? Difficulty { get; set; } // Navigation Properties // So here .net is smart enough to relate this table


        public Guid RegionId { get; set; }
        public Region? Region { get; set; } // Navigation Properties // So here .net is smart enough to relate this table
    }
}
