using System.Net;

namespace Quiz_PROJECT.Errors;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, string description) 
        : base(message, description, (int)HttpStatusCode.NotFound)
    {
    }
}