using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quiz_PROJECT.Services;
using Quiz_PROJECT.Services.JWT;
using Quiz_PROJECT.Services.PasswordHashAndSalt;

namespace Quiz_PROJECT.Configurations;

public static class ServicesManager
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IMailKitService, MailKitService>();

        services.AddScoped<ITokenHelper, TokenHelper>();
        services.AddScoped<IPasswordHash, PasswordHash>();

        services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
    }
}