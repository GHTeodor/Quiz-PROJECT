using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models.DTOModels;

public class SendMailDTO
{
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string To { get; set; } = "example@gmail.com";
    public string Subject { get; set; } = "Title";
    public string Body { get; set; } = "<h1>Hello from MailKit<h1>";
}