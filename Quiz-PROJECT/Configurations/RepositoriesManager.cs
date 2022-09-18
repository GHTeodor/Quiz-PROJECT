using Quiz_PROJECT.Models;
using Quiz_PROJECT.Repositories;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Configurations;

public static class RepositoriesManager
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRepository<Question>, QuestionRepository>();
        services.AddTransient<IRepository<Answer>, AnswerRepository>();
    }
}