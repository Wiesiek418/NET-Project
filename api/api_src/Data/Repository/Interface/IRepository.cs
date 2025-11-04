using Data.Models;

namespace Data.Repository;

public interface IRepository<T> where T : BaseReading
{
    Task CreateAsync(T reading);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
}