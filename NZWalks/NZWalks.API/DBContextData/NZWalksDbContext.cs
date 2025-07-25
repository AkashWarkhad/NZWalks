using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties {get; set;}

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Code column is now allowed only unique fields
            modelBuilder.Entity<Region>()
            .HasIndex(u => u.Code)
            .IsUnique();

            // Seeding data for difficulties
            var difficulties = new List<Difficulty>()
            {
                new Difficulty
                {
                    Id = Guid.Parse("78aa5093-9702-449d-a59e-439b84392793"),
                    Name = "Easy"
                },
                new Difficulty
                {
                    Id = Guid.Parse("3e127810-303a-462f-bc85-8872a640ca31"),
                    Name = "Medium"
                },
                new Difficulty
                {
                    Id = Guid.Parse("620476b7-0750-42aa-8dcf-b7ac429906d1"),
                    Name = "Hard"
                }
            };

            // Seed the data into difficulty
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seeding data for regions
            var regions = new List<Region>()
            {
                new Region
                {
                    Id = Guid.Parse("69bd489f-0c4e-4353-b5ae-e1d65f63914b"),
                    Code = "Ak",
                    Name = "Akash Shankar Warkhad",
                    RegionImageUrl = "https://images.app.goo.gl/2WPv5cwB3Eeyb1AH9"
                },
                new Region
                {
                    Id = Guid.Parse("99aec440-50d8-4638-a4e8-4c59c68e9942"),
                    Code = "SH",
                    Name = "Shubham Shankar Warkhad",
                    RegionImageUrl = "https://images.app.goo.gl/qChsgyYUzHhcFjfH8"
                },
                new Region
                {
                    Id = Guid.Parse("2946b5ca-be41-46fb-9b92-b60183732a6a"),
                    Code = "RK",
                    Name = "Rushikesh Shankar Warkhad",
                    RegionImageUrl = "https://images.app.goo.gl/ArUnGebmQj28VhS36"
                }
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }

    }
}
