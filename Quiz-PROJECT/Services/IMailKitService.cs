using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IMailKitService
{
    Task<string> SendMail(SendMailDTO mail);
}