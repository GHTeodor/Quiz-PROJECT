using System.Net;
using Newtonsoft.Json;

namespace Quiz_PROJECT.Errors;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BadRequestException e)
        {
            await HandleExceptionAsync(context, e);
        }
        catch (NotFoundException e)
        {
            await HandleExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        var customException = exception as BaseException;
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Unexpected error";
        var description = "Unexpected error";

        if (customException != null)
        {
            message = customException.Message;
            description = customException.Description;
            statusCode = customException.Code;
        }

        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        await response.WriteAsync(new ErrorResponse
        {
            Message = message,
            Description = description
        }.ToString());
    }
    private class ErrorResponse
    {
        public string Message { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}