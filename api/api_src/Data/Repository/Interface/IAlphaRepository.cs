using Data.Models;

namespace Data.Repository;

public interface IAlphaRepository : IRepository<AlphaReading>
{
    Task<IEnumerable<AlphaReading>> GetAllByValueAsync(int value);
}