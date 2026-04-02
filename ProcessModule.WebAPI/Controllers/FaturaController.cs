using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Features.Fatura.Commands;
using ProcessModule.Application.Features.Fatura.Queries;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FaturaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFaturaPdfService _pdfService;
    private readonly IFaturaRepository _faturaRepository;

    public FaturaController(
        IMediator mediator, 
        IFaturaPdfService pdfService,
        IFaturaRepository faturaRepository)
    {
        _mediator = mediator;
        _pdfService = pdfService;
        _faturaRepository = faturaRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<FaturaDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllFaturaQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FaturaDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetFaturaByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("cari/{cariHesapId}")]
    public async Task<ActionResult<List<FaturaDto>>> GetByCariHesap(int cariHesapId)
    {
        var result = await _mediator.Send(new GetFaturaByCariHesapQuery { CariHesapId = cariHesapId });
        return Ok(result);
    }

    [HttpGet("generate-fatura-no")]
    public async Task<ActionResult<string>> GenerateNextFaturaNo()
    {
        var result = await _mediator.Send(new GenerateNextFaturaNoQuery());
        return Ok(new { faturaNo = result });
    }

    [HttpPost]
    public async Task<ActionResult<FaturaDto>> Create([FromBody] CreateFaturaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new CreateFaturaCommand { Fatura = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FaturaDto>> Update(int id, [FromBody] UpdateFaturaDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(new UpdateFaturaCommand { Fatura = dto });
            
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
        try
        {
            var result = await _mediator.Send(new DeleteFaturaCommand { Id = id });
            
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> DownloadPdf(int id)
    {
        try
        {
            // Fatura verilerini çek (navigation properties dahil)
            var fatura = await _faturaRepository.GetByIdWithDetailsAsync(id);
            
            if (fatura == null)
                return NotFound(new { message = "Fatura bulunamadı" });

            // PDF oluştur
            var pdfBytes = _pdfService.GenerateFaturaPdf(fatura);

            // PDF'i döndür
            var fileName = $"Fatura_{fatura.FaturaNo}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"PDF oluşturulurken hata: {ex.Message}" });
        }
    }
}
