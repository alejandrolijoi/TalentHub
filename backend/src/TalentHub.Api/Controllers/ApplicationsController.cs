using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.DTOs.Applications;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ICandidateService _candidateService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public ApplicationsController(IApplicationService applicationService, ICandidateService candidateService)
    {
        _applicationService = applicationService;
        _candidateService = candidateService;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> ApplyToJob([FromBody] ApplyToJobRequest request)
    {
        var candidate = await _candidateService.GetByUserIdAsync(UserId);
        if (!candidate.IsSuccess) return BadRequest(candidate.Error);

        var result = await _applicationService.ApplyAsync(candidate.Value!.Id, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetMyApplications), new { }, result.Value);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> GetMyApplications([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var candidate = await _candidateService.GetByUserIdAsync(UserId);
        if (!candidate.IsSuccess) return BadRequest(candidate.Error);

        var result = await _applicationService.GetByCandidateIdAsync(candidate.Value!.Id, page, pageSize);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("my/stats")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> GetMyStats()
    {
        var candidate = await _candidateService.GetByUserIdAsync(UserId);
        if (!candidate.IsSuccess) return BadRequest(candidate.Error);

        var result = await _applicationService.GetStatsByCandidateIdAsync(candidate.Value!.Id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateApplicationStatusRequest request)
    {
        var companyService = HttpContext.RequestServices.GetRequiredService<ICompanyService>();
        var company = await companyService.GetByUserIdAsync(UserId);
        if (!company.IsSuccess) return BadRequest(company.Error);

        var result = await _applicationService.UpdateStatusAsync(company.Value!.Id, id, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("job/{jobId}")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetByJobId(Guid jobId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var companyService = HttpContext.RequestServices.GetRequiredService<ICompanyService>();
        var company = await companyService.GetByUserIdAsync(UserId);
        if (!company.IsSuccess) return BadRequest(company.Error);

        var result = await _applicationService.GetByJobIdAsync(company.Value!.Id, jobId, page, pageSize);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpDelete("{id}/withdraw")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> Withdraw(Guid id)
    {
        var candidate = await _candidateService.GetByUserIdAsync(UserId);
        if (!candidate.IsSuccess) return BadRequest(candidate.Error);

        var result = await _applicationService.WithdrawAsync(candidate.Value!.Id, id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
