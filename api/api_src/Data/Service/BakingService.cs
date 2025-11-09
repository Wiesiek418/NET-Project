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

    public Task<IEnumerable<BakingFurnaceReading>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }

    public Task CreateAsync(BakingFurnaceReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
