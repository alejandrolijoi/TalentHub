using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? query)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            var searchResult = await _skillService.SearchAsync(query);
            if (!searchResult.IsSuccess) return BadRequest(searchResult.Error);
            return Ok(searchResult.Value);
        }

        var result = await _skillService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }
}
