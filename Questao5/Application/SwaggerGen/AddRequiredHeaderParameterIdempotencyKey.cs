using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Questao5.Application.SwaggerGen
{
    public class AddRequiredHeaderParameterIdempotencyKey : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.CustomAttributes
                .Any(x => x.AttributeType == typeof(IdempotentAPI.Filters.IdempotentAttribute)))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "IdempotencyKey",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "String" },
                    Example = new OpenApiString(Guid.NewGuid().ToString())
                });
            }
        }
    }
}
