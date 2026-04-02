using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Features.CariHesap.Commands;
using ProcessModule.Application.Features.CariHesap.Queries;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CariHesapController : ControllerBase
{
    private readonly IMediator _mediator;

    public CariHesapController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CariHesapDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCariHesapQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CariHesapDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetCariHesapByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CariHesapDto>> Create([FromBody] CreateCariHesapDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new CreateCariHesapCommand { CariHesap = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CariHesapDto>> Update(int id, [FromBody] UpdateCariHesapDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new UpdateCariHesapCommand { CariHesap = dto });
            
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCariHesapCommand { Id = id });
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
