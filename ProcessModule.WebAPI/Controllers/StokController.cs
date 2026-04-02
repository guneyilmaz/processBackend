using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Features.Stok.Commands;
using ProcessModule.Application.Features.Stok.Queries;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StokController : ControllerBase
{
    private readonly IMediator _mediator;

    public StokController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<StokDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllStokQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StokDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetStokByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<List<StokDto>>> GetLowStockItems()
    {
        var result = await _mediator.Send(new GetLowStockItemsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<StokDto>> Create([FromBody] CreateStokDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new CreateStokCommand { Stok = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StokDto>> Update(int id, [FromBody] UpdateStokDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new UpdateStokCommand { Stok = dto });
            
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
        var result = await _mediator.Send(new DeleteStokCommand { Id = id });
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
