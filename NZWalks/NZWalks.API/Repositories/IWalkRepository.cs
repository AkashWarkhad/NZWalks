using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> DeleteWalksById(Guid id);

        Task<List<Walk>?> GetWalkAsync(
            string? filterOn = null, 
            string? filterQuery = null,
            string? sortBy = null,
            bool isAsceding = false,
            int pageNumber = 1,
            int pageSize = 1000);

        Task<Walk?> GetWalkByIdAsync(Guid id);

        Task<Walk?> UpdateWalkByIdAsync(Guid id, Walk walkDomainModel);
    }
}
