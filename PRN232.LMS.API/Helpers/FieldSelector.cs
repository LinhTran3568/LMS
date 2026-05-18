using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace PRN232.LMS.API.Helpers;

public static class FieldSelector
{
    public static object? ApplyFields(object? source, string? fields)
    {
        if (source == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(fields))
        {
            return source;
        }

        var fieldSet = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (source is IEnumerable enumerable and not string)
        {
            var list = new List<object?>();
            foreach (var item in enumerable)
            {
                list.Add(ApplyFields(item, fields));
            }
            return list;
        }

        return FilterObject(source, fieldSet);
    }

    private static Dictionary<string, object?> FilterObject(object source, HashSet<string> fieldSet)
    {
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        var type = source.GetType();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead)
            {
                continue;
            }

            var jsonName = JsonNamingPolicy.CamelCase.ConvertName(property.Name);
            if (!fieldSet.Contains(jsonName) && !fieldSet.Contains(property.Name))
            {
                continue;
            }

            var value = property.GetValue(source);
            if (value is IEnumerable enumerable and not string)
            {
                var nestedFields = fieldSet
                    .Where(f => f.Contains('.'))
                    .Select(f => f[(f.IndexOf('.') + 1)..])
                    .Where(f => !string.IsNullOrWhiteSpace(f))
                    .ToArray();
                var childFields = nestedFields.Length > 0 ? string.Join(',', nestedFields) : null;
                result[jsonName] = ApplyFields(enumerable, childFields);
            }
            else
            {
                result[jsonName] = value;
            }
        }

        return result;
    }
}
