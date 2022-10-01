using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Configurations;

public static class ServicesManager
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMailKitService, MailKitService>();
    }
}