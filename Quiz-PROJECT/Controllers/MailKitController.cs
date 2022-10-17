using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class MailKitController: ControllerBase
{
    private readonly IMailKitService _mailKitService;

    public MailKitController(IMailKitService mailKitService)
    {
        _mailKitService = mailKitService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync(SendMailDTO mail, CancellationToken token = default)
    {
        return Ok(await _mailKitService.SendMail(mail, token));
    }
    
    [HttpGet(@"{email:regex(^[[\w\.]]+@([[\w-]]+\.)+[[\w-]]{{2,4}}$)}")]
    public async Task<IActionResult> ConfirmEmail(string email, CancellationToken token = default)
    {
        return Ok(await _mailKitService.SendToConfirmEmail(email, token));
    }
}