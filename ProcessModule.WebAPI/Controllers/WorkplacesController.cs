using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProcessModule.Application.Features.Workplaces.Commands;
using ProcessModule.Application.Features.Workplaces.Queries;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkplacesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkplacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllWorkplacesQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetWorkplaceByIdQuery { Id = id });
            if (result == null)
                return NotFound(new { message = "İşyeri bulunamadı." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("by-company/{companyId}")]
    public async Task<IActionResult> GetByCompanyId(int companyId)
    {
        try
        {
            var result = await _mediator.Send(new GetWorkplacesByCompanyIdQuery { CompanyId = companyId });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkplaceCommand command)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Bir hata oluştu.", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkplaceCommand command)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != command.Id)
                return BadRequest(new { message = "ID uyuşmazlığı." });

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Bir hata oluştu.", details = ex.Message });
        }
    }
}