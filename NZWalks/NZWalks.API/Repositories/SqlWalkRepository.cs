using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        
        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            try
            {
                await _dbContext.Walks.AddAsync(walk);
                await _dbContext.SaveChangesAsync();
                return walk;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
            }
           
        }

        public async Task<Walk?> DeleteWalksById(Guid id)
        {
            var deletedWalks = await _dbContext.Walks.FirstOrDefaultAsync(walk => walk.Id == id);
            if (deletedWalks != null) 
            {
                _dbContext.Walks.Remove(deletedWalks);
                await _dbContext.SaveChangesAsync();
                return deletedWalks;
            }

            return null;
        }

        public async Task<List<Walk>?> GetWalkAsync(
            string? filterOn, 
            string? filterQuery,
            string? sortBy, 
            bool isAscending,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            try
            {
                // retrieved walks from the database
                var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

                // Filtering based on column & query
                if (string.IsNullOrEmpty(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
                {
                    if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = walks.Where(x => x.Name.Contains(filterQuery));
                    }
                    else
                    {
                        walks = null;
                    }
                }

                // Sorting
                if (string.IsNullOrEmpty(sortBy) == false && walks != null)
                {
                    if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending == true ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                    }
                    else if(sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending == true ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                    }
                }

                // Pagination
                var skipResult = (pageNumber - 1) * pageSize;

                return walks != null ? await walks.Skip(skipResult).Take(pageSize).ToListAsync() : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine
                    + ex.InnerException?.Message);
            }
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine
                    + ex.InnerException?.Message);
            }
        }

        public async Task<Walk?> UpdateWalkByIdAsync(Guid id, Walk walkDomainModel)
        {
            var existingWalks = await _dbContext.Walks.FirstOrDefaultAsync(x=> x.Id == id);

            if (existingWalks == null) return null;

            existingWalks.Name = walkDomainModel.Name;
            existingWalks.Description = walkDomainModel.Description;
            existingWalks.LengthInKm = walkDomainModel.LengthInKm;
            existingWalks.RegionId = walkDomainModel.RegionId;
            existingWalks.DifficultyId = walkDomainModel.DifficultyId;
            existingWalks.WalkImageUrl = walkDomainModel.WalkImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingWalks;
        }
    }
}
