using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class MemoryRegionRepository : IRegionRepository
    {
        public Task<List<Region>> GetAllAsync()
        {
            return Task.FromResult<List<Region>>([
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "DummyCode1",
                    Name = "Fake Name 123",
                    RegionImageUrl = "Temp1.img"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "DummyCode2",
                    Name = "Fake Name 456",
                    RegionImageUrl = "Temp2.img"
                }
            ]);
        }

        public Task<Region?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region> CreateAsync(Region regionDomain)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid id, Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
