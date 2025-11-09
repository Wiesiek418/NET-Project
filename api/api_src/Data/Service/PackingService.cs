using Data.Models;
using Data.Repository;
using Data.Services.Interfaces;

namespace Data.Services;

public class PackingService : IPackingService
{
    private readonly IPackingRepository _repo;

    public PackingService(IPackingRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<PackingLineReading>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }

    public Task CreateAsync(PackingLineReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
