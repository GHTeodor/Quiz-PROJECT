using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await Task.FromResult<IActionResult>(Ok(await _answerService.Get()));
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id)
    {
        return await Task.FromResult<IActionResult>(Ok(await _answerService.GetById(id)));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateAnswerDTO answer)
    {
        Answer newAnswer = await _answerService.Post(answer);

        return await Task.FromResult<IActionResult>(Accepted(newAnswer));
    }    
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromBody] UpdateAnswerDTO answer, int id)
    {
        Answer updatedAnswer = await _answerService.Put(answer, id);
        return await Task.FromResult<IActionResult>(Ok(updatedAnswer));
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        await _answerService.DeleteById(id);
        return await Task.FromResult<IActionResult>(Ok(id));
    }
}