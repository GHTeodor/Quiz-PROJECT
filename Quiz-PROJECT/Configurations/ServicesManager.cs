using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Configurations;

public static class ServicesManager
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<IQuestionService, QuestionService>();
    }
}