using Data.Models;
using Data.Repository;
using Data.Services.Interfaces;

namespace Data.Services;

public class DoughService : IDoughService
{
    private readonly IDoughRepository _repo;

    public DoughService(IDoughRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<DoughMixerReading>> GetAllAsync(string? filter, string? sort)
    {
        return _repo.GetAllAsync(filter, sort);
    }

    public Task CreateAsync(DoughMixerReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
