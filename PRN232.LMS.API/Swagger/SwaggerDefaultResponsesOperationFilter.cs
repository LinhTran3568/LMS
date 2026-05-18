using Microsoft.OpenApi.Models;
using PRN232.LMS.API.Models.Common;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PRN232.LMS.API.Swagger;

public class SwaggerDefaultResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.ContainsKey("500"))
        {
            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Internal Server Error"
            });
        }

        operation.Responses["500"].Content ??= new Dictionary<string, OpenApiMediaType>();
        if (!operation.Responses["500"].Content.ContainsKey("application/json"))
        {
            operation.Responses["500"].Content["application/json"] = new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse<object>), context.SchemaRepository)
            };
        }
    }
}
