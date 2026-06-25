using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.DTOs.Companies;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("me")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetMyCompany()
    {
        var result = await _companyService.GetByUserIdAsync(UserId);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> UpdateMyCompany([FromBody] UpdateCompanyRequest request)
    {
        var result = await _companyService.UpdateAsync(UserId, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyProfile(Guid id)
    {
        var result = await _companyService.GetPublicProfileAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> SearchCompanies([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _companyService.SearchAsync(query, page, pageSize);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }
}
