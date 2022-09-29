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
            
            // c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test Identity Server API", Version = "v1" });
            //
            // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     Description =
            //         "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //     Name = "Authorization",
            //     In = ParameterLocation.Header,
            //     Type = SecuritySchemeType.ApiKey
            // });
            //
            // c.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference
            //             {
            //                 Type = ReferenceType.SecurityScheme,
            //                 Id = "Bearer"
            //             }
            //         },
            //         new string[] { }
            //     }
            // });
        });
    }
}