using System.Net;

namespace Quiz_PROJECT.Errors;

public class BadRequestException : BaseException
{
    public BadRequestException(string message, string description) 
        : base(message, description, (int)HttpStatusCode.BadRequest)
    { }
}