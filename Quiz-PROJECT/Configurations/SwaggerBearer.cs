using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Quiz_PROJECT.Configurations;

public static class SwaggerBearer
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }
}