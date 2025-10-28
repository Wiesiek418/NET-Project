using Data.Models;

namespace Data.Services.Interfaces;

public interface IReadingService<T> where T : BaseReading
{
    Task CreateAsync(T reading);
    Task<IEnumerable<T>> GetAllAsync();
}