using System.Globalization;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repository;

public class MongoRepository<T> : IRepository<T> where T : BaseReading
{
    protected readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task<IEnumerable<T>> GetAllAsync(string? filter, string? sort)
    {
        var filterDef = BuildFilterDefinition(filter);
        var sortDef = BuildSortDefinition(sort);

        var query = _collection.Find(filterDef);

        if (sortDef != null) query = query.Sort(sortDef);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    private SortDefinition<T> BuildSortDefinition(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
            // Default sort by timestamp if none provided
            return Builders<T>.Sort.Descending(nameof(BaseReading.Timestamp));

        var parts = sort.Split(':');
        if (parts.Length != 2)
            // Fallback on invalid format
            return Builders<T>.Sort.Descending(nameof(BaseReading.Timestamp));

        var fieldName = parts[0];
        var direction = parts[1].ToLowerInvariant();

        return (direction == "desc") 
            ? Builders<T>.Sort.Descending(fieldName) 
            : Builders<T>.Sort.Ascending(fieldName);
    }

    private FilterDefinition<T> BuildFilterDefinition(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) return Builders<T>.Filter.Empty;

        var filters = new List<FilterDefinition<T>>();
        var criteria = filter.Split(',');

        foreach (var criterion in criteria)
        {
            var parts = criterion.Split(':');
            if (parts.Length != 3) continue; // Skip invalid format

            var field = parts[0];
            var op = parts[1].ToLowerInvariant();
            var value = parts[2];

            // Attempt to parse value as double for numeric comparisons
            double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue);

            switch (op)
            {
                case "eq":
                    filters.Add(Builders<T>.Filter.Eq(field, value));
                    break;
                case "gt":
                    filters.Add(Builders<T>.Filter.Gt(field, numericValue));
                    break;
                case "gte":
                    filters.Add(Builders<T>.Filter.Gte(field, numericValue));
                    break;
                case "lt":
                    filters.Add(Builders<T>.Filter.Lt(field, numericValue));
                    break;
                case "lte":
                    filters.Add(Builders<T>.Filter.Lte(field, numericValue));
                    break;
                case "contains":
                    // Case-insensitive contains
                    filters.Add(Builders<T>.Filter.Regex(field, new BsonRegularExpression(value, "i")));
                    break;
            }
        }

        return (filters.Count == 0) ? Builders<T>.Filter.Empty : Builders<T>.Filter.And(filters);
    }
}