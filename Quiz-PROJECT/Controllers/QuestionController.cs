using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken token = default)
    {
        return Ok(await _questionService.GetAllAsync(token));
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken token = default)
    {
        return Ok(await _questionService.GetByIdAsync(id, token));
    }

    [HttpPost]
    public async Task<IActionResult> PostCreateAsync([FromBody] CreateQuestionDTO question, CancellationToken token = default)
    {
        return Accepted(await _questionService.CreateAsync(question, token));
    }
    
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> PutUpdateByIdAsync([FromBody] UpdateQuestionDTO question, long id, CancellationToken token = default)
    {
        return Ok(await _questionService.UpdateByIdAsync(question, id, token));
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id, CancellationToken token = default)
    {
        await _questionService.DeleteByIdAsync(id, token);
        return Ok(id);
    }
}