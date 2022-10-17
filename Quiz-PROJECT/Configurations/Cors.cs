namespace Quiz_PROJECT.Configurations;

public static class Cors
{
    public static void AddCorsS(this IServiceCollection services)
    {
        services.AddCors(options => options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        }));
    }
}