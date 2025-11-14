using Data.Models;
using Data.Repository;
using Data.Services.Interfaces;

namespace Data.Services;

public class BakingService : IBakingService
{
    private readonly IBakingRepository _repo;

    public BakingService(IBakingRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<BakingFurnaceReading>> GetAllAsync(string? filter, string? sort)
    {
        return _repo.GetAllAsync(filter, sort);
    }

    public Task CreateAsync(BakingFurnaceReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
