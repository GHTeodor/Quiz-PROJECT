using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Quiz_PROJECT.Configurations;

public static class Authentication
{
    public static void AddAuthenticationS(this IServiceCollection services, string configurationValue)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationValue)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }
}