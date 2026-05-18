using PRN232.LMS.Repositories.Common;

namespace PRN232.LMS.Services.Common;

public static class QueryMapper
{
    public static DataQueryOptions ToDataQuery(ListQueryBusiness query) => new()
    {
        Search = query.Search,
        Sort = query.Sort,
        Page = query.Page,
        PageSize = query.PageSize
    };
}
