namespace PRN232.LMS.Services.Common;

public class ListQueryBusiness
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Fields { get; set; }
    public string? Expand { get; set; }

    public HashSet<string> GetExpandSet() =>
        string.IsNullOrWhiteSpace(Expand)
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : Expand.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public bool ShouldExpand(string name) => GetExpandSet().Contains(name);
}
