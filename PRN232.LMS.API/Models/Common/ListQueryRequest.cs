using PRN232.LMS.Services.Common;

namespace PRN232.LMS.API.Models.Common;

public class ListQueryRequest
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Fields { get; set; }
    public string? Expand { get; set; }

    public ListQueryBusiness ToBusiness() => new()
    {
        Search = Search,
        Sort = Sort,
        Page = Page,
        PageSize = Size,
        Fields = Fields,
        Expand = Expand
    };
}
