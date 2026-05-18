namespace PRN232.LMS.Repositories.Common;

public class DataQueryOptions
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
