using System.Net;

namespace Quiz_PROJECT.Errors;

public class UnauthorizedException: BaseException
{
    public UnauthorizedException(string message, string description) 
        : base(message, description, (int)HttpStatusCode.Unauthorized)
    { }
}