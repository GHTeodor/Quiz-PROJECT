using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class MailKitController: ControllerBase
{
    private readonly IMailKitService _mailKitService;

    public MailKitController(IMailKitService mailKitService)
    {
        _mailKitService = mailKitService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync(SendMailDTO mail)
    {
        return Ok(await _mailKitService.SendMail(mail));
    }
}