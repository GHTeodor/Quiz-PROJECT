using System.Net;

namespace Quiz_PROJECT.Errors;

public class ForbiddenException: BaseException
{
    public ForbiddenException(string message, string description) 
        : base(message, description, (int)HttpStatusCode.Forbidden)
    { }
}