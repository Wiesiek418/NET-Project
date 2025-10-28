using Data.Models;
using Data.Repository;
using Data.Services.Interfaces;

namespace Data.Services;

public class AlphaService : IAlphaService
{
    private readonly IAlphaRepository _repo;

    public AlphaService(IAlphaRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<AlphaReading>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }

    public Task CreateAsync(AlphaReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}