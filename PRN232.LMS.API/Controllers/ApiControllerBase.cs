using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Helpers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected static object ApplyFields(object source, string? fields) =>
        FieldSelector.ApplyFields(source, fields) ?? source;

    protected static ApiResponse<object> PagedOk<TResponse>(
        PagedResultBusiness<TResponse> paged,
        Func<TResponse, object> map,
        string? fields)
    {
        var items = paged.Items.Select(map).ToList();
        var payload = new PagedListResponse<object>
        {
            Items = items.Select(i => ApplyFields(i, fields)).Cast<object>().ToList(),
            Pagination = new PaginationResponse
            {
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalItems = paged.TotalItems,
                TotalPages = paged.TotalPages
            }
        };

        return ApiResponse<object>.Ok(payload);
    }
}
