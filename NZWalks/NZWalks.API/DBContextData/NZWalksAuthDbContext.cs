using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;

namespace NZWalks.API.DBContextData
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensures configurations from the base DbContext class (like IdentityDbContext) are not lost or overridden.


            var readeRoleId = "78aa5093-9702-449d-a59e-439b84392793";
            var writerRoleId = "88aa5093-9702-449d-a59e-439b84392794";

            var roles = new List<IdentityRole>()
            {
               new IdentityRole()
               {
                   Id = readeRoleId,
                   ConcurrencyStamp = readeRoleId,
                   Name = "Reader",
                   NormalizedName = "Reader".ToUpper()
               },
               new IdentityRole()
               {
                   Id = writerRoleId,
                   ConcurrencyStamp = writerRoleId,
                   Name = "Writer",
                   NormalizedName = "Writer".ToUpper()
               }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
