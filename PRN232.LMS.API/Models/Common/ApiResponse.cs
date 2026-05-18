namespace PRN232.LMS.API.Models.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Request processed successfully.") => new()
    {
        Success = true,
        Message = message,
        Data = data,
        Errors = null
    };

    public static ApiResponse<T> Fail(string message, object? errors = null) => new()
    {
        Success = false,
        Message = message,
        Data = default,
        Errors = errors
    };
}

public class PaginationResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class PagedListResponse<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public PaginationResponse Pagination { get; set; } = new();
}
