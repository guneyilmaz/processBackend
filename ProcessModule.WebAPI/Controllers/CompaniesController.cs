using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProcessModule.Application.Features.Companies.Commands;
using ProcessModule.Application.Features.Companies.Queries;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllCompaniesQuery());
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
            var result = await _mediator.Send(new GetCompanyByIdQuery { Id = id });
            if (result == null)
                return NotFound(new { message = "Firma bulunamadı." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("with-workplaces")]
    public async Task<IActionResult> GetCompaniesWithWorkplaces()
    {
        try
        {
            var result = await _mediator.Send(new GetCompaniesWithWorkplacesQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyCommand command)
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