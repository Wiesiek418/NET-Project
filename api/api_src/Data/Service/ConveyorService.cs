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

    public Task<IEnumerable<ConveyorBeltReading>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }

    public Task CreateAsync(ConveyorBeltReading reading)
    {
        return _repo.CreateAsync(reading);
    }
}
