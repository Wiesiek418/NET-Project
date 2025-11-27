using System.Globalization;
using System.Reflection;

namespace Extensions;

public static class EnumerableQueryExtensions
{
    public static IEnumerable<T> ApplyQuery<T>(this IEnumerable<T> source, string? filter, string? sort)
    {
        var query = source;

        // Filtering
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var criteria = filter.Split(',');
            foreach (var criterion in criteria)
            {
                var parts = criterion.Split(':');
                if (parts.Length != 3) continue;

                var fieldName = parts[0];
                var op = parts[1].ToLowerInvariant();
                var value = parts[2];

                var prop = typeof(T).GetProperty(fieldName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                query = query.Where(item => ApplyPredicate(item, prop, op, value));
            }
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(sort))
        {
            var parts = sort.Split(':');
            if (parts.Length == 2)
            {
                var fieldName = parts[0];
                var direction = parts[1].ToLowerInvariant();
                var prop = typeof(T).GetProperty(fieldName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop != null)
                    query = direction == "desc"
                        ? query.OrderByDescending(x => prop.GetValue(x))
                        : query.OrderBy(x => prop.GetValue(x));
            }
        }

        return query;
    }

    private static bool ApplyPredicate<T>(T item, PropertyInfo prop, string op, string value)
    {
        var itemValue = prop.GetValue(item);
        if (itemValue == null) return false;

        double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericFilterValue);
        var numericItemValue = ConvertToDouble(itemValue);

        return op switch
        {
            "eq" => itemValue.ToString()?.Equals(value, StringComparison.InvariantCultureIgnoreCase) ?? false,
            "contains" => itemValue.ToString()?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false,
            "gt" when numericItemValue.HasValue => numericItemValue > numericFilterValue,
            "gte" when numericItemValue.HasValue => numericItemValue >= numericFilterValue,
            "lt" when numericItemValue.HasValue => numericItemValue < numericFilterValue,
            "lte" when numericItemValue.HasValue => numericItemValue <= numericFilterValue,
            _ => true
        };
    }

    private static double? ConvertToDouble(object value)
    {
        if (value is int i) return i;
        if (value is double d) return d;
        if (value is float f) return f;
        if (value is long l) return l;
        if (value is decimal m) return (double)m;
        return null;
    }
}