using System.Globalization;
using Core.Abstractions;
using Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoDB;

/// <summary>
///     MongoDB-specific repository implementation.
///     Isolates MongoDB Driver from domain logic.
/// </summary>
public class MongoRepository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(string? filter, string? sort, CancellationToken ct = default)
    {
        var filterDef = BuildFilterDefinition(filter);
        var sortDef = BuildSortDefinition(sort);

        var query = _collection.Find(filterDef);

        if (sortDef != null) query = query.Sort(sortDef);

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        // Assumes entity has an 'Id' property
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync(ct);
    }

    public virtual async Task CreateAsync(T entity, CancellationToken ct = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: ct);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        // Assumes entity has an 'Id' property
        var id = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString();
        if (string.IsNullOrEmpty(id)) throw new InvalidOperationException("Entity must have an Id");

        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: ct);
    }

    public virtual async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter, ct);
    }

    private SortDefinition<T> BuildSortDefinition(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
            // Default sort by id if none provided
            return Builders<T>.Sort.Descending(nameof(Entity.Id));

        var parts = sort.Split(':');
        if (parts.Length != 2)
            // Fallback on invalid format
            return Builders<T>.Sort.Descending(nameof(Entity.Id));

        var fieldName = parts[0];
        var direction = parts[1].ToLowerInvariant();

        return direction == "desc"
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

            var isNumber = double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue);

            switch (op)
            {
                case "eq":
                    filters.Add(isNumber
                        ? Builders<T>.Filter.Eq(field, numericValue)
                        : Builders<T>.Filter.Eq(field, value));
                    break;
                case "gt":
                    filters.Add(isNumber
                        ? Builders<T>.Filter.Gt(field, numericValue)
                        : Builders<T>.Filter.Eq(field, value));
                    break;
                case "gte":
                    filters.Add(isNumber
                        ? Builders<T>.Filter.Gte(field, numericValue)
                        : Builders<T>.Filter.Eq(field, value));
                    break;
                case "lt":
                    filters.Add(isNumber
                        ? Builders<T>.Filter.Lt(field, numericValue)
                        : Builders<T>.Filter.Eq(field, value));
                    break;
                case "lte":
                    filters.Add(isNumber
                        ? Builders<T>.Filter.Lte(field, numericValue)
                        : Builders<T>.Filter.Eq(field, value));
                    break;
                case "contains":
                    filters.Add(Builders<T>.Filter.Regex(field, new BsonRegularExpression(value, "i")));
                    break;
            }
        }

        return filters.Count == 0 ? Builders<T>.Filter.Empty : Builders<T>.Filter.And(filters);
    }
}