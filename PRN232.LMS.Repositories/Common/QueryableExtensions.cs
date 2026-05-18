using System.Linq.Expressions;
using System.Reflection;

namespace PRN232.LMS.Repositories.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
    {
        var safePage = page < 1 ? 1 : page;
        var safeSize = pageSize < 1 ? 10 : pageSize;
        return query.Skip((safePage - 1) * safeSize).Take(safeSize);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query;
        }

        IOrderedQueryable<T>? ordered = null;
        foreach (var segment in sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var descending = segment.StartsWith('-');
            var propertyName = descending ? segment[1..] : segment;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                continue;
            }

            var property = ResolveProperty(typeof(T), propertyName);
            if (property == null)
            {
                continue;
            }

            ordered = ApplyOrder(query, ordered, property, descending);
            query = ordered ?? query;
        }

        return ordered ?? query;
    }

    private static PropertyInfo? ResolveProperty(Type type, string name)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(
        IQueryable<T> source,
        IOrderedQueryable<T>? ordered,
        PropertyInfo property,
        bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var lambda = Expression.Lambda(propertyAccess, parameter);
        var methodName = ordered == null
            ? (descending ? "OrderByDescending" : "OrderBy")
            : (descending ? "ThenByDescending" : "ThenBy");

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.PropertyType);

        var result = method.Invoke(null, new object?[] { ordered ?? source, lambda });
        return (IOrderedQueryable<T>)result!;
    }
}
