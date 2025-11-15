using Data.Models;
using Data.Repository;
using Data.Services.Interfaces;

namespace Data.Services;

public class ConveyorService : IConveyorService
{
    private readonly IConveyorRepository _repo;

    public ConveyorService(IConveyorRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<ConveyorBeltReading>> GetAllAsync(string? filter, string? sort)
    {
        return _repo.GetAllAsync(filter, sort);
    }

    public Task CreateAsync(ConveyorBeltReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
