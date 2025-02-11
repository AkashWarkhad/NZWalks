using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstAsync(x=> x.Id == id);
        }

        public async Task<Region> CreateAsync(Region regionDomain)
        {
           await _dbContext.AddAsync(regionDomain);
           await _dbContext.SaveChangesAsync();
           return regionDomain;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegionDomainModel == null)
            {
                return null;
            }

            existingRegionDomainModel.Code = region.Code;
            existingRegionDomainModel.Name = region.Name;
            existingRegionDomainModel.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingRegionDomainModel;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegionModel = await _dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);

            if (existingRegionModel == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(existingRegionModel);
            await _dbContext.SaveChangesAsync();

            return existingRegionModel;
        }
    }
}
