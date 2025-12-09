using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessModule.Application.Features.Menus.Queries;

namespace ProcessModule.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenusController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get menus with translations for specified language
    /// </summary>
    /// <param name="languageCode">Language code (tr, en, etc.)</param>
    /// <returns>List of menus with hierarchical structure</returns>
    [HttpGet]
    public async Task<ActionResult<List<MenuDto>>> GetMenus([FromQuery] string languageCode = "tr")
    {
        try
        {
            // Use default "tr" if languageCode is null or empty
            if (string.IsNullOrEmpty(languageCode))
                languageCode = "tr";
                
            var query = new GetMenusQuery { LanguageCode = languageCode };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching menus", error = ex.Message });
        }
    }

    /// <summary>
    /// Get menus for Turkish language (shortcut)
    /// </summary>
    [HttpGet("tr")]
    public async Task<ActionResult<List<MenuDto>>> GetTurkishMenus()
    {
        return await GetMenus("tr");
    }

    /// <summary>
    /// Get menus for English language (shortcut)
    /// </summary>
    [HttpGet("en")]
    public async Task<ActionResult<List<MenuDto>>> GetEnglishMenus()
    {
        return await GetMenus("en");
    }
}