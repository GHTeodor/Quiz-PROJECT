using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Quiz_PROJECT.Configurations;

public static class Authentication
{
    public static void AddAuthenticationS(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
            .AddGoogle(googleOptions => // signin-google (dotnet user-secrets)
            {
                // dotnet user-secrets set "AppSettings:Authentication:Google:ClientId" "________________.apps.googleusercontent.com"
                // dotnet user-secrets set "AppSettings:Authentication:Google:ClientSecret" "____________________"
                googleOptions.ClientId = configuration["AppSettings:Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["AppSettings:Authentication:Google:ClientSecret"];
            });
    }
}