using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.DTOs.Jobs;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchJobs([FromQuery] JobSearchRequest request)
    {
        var result = await _jobService.SearchAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedJobs([FromQuery] int limit = 10)
    {
        var result = await _jobService.GetFeaturedJobsAsync(limit);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJob(Guid id)
    {
        var result = await _jobService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        await _jobService.IncrementViewCountAsync(id);
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        var companyResult = await GetCompanyId();
        if (!companyResult.IsSuccess) return BadRequest(companyResult.Error);

        var result = await _jobService.CreateAsync(companyResult.Value, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetJob), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequest request)
    {
        var companyResult = await GetCompanyId();
        if (!companyResult.IsSuccess) return BadRequest(companyResult.Error);

        var result = await _jobService.UpdateAsync(companyResult.Value, id, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var companyResult = await GetCompanyId();
        if (!companyResult.IsSuccess) return BadRequest(companyResult.Error);

        var result = await _jobService.DeleteAsync(companyResult.Value, id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpGet("my")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetMyJobs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var companyResult = await GetCompanyId();
        if (!companyResult.IsSuccess) return BadRequest(companyResult.Error);

        var result = await _jobService.GetByCompanyIdAsync(companyResult.Value, page, pageSize);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    private async Task<Application.DTOs.Common.Result<Guid>> GetCompanyId()
    {
        var companyService = HttpContext.RequestServices.GetRequiredService<ICompanyService>();
        var result = await companyService.GetByUserIdAsync(UserId);
        if (!result.IsSuccess) return Application.DTOs.Common.Result<Guid>.Failure("Company not found");
        return Application.DTOs.Common.Result<Guid>.Success(result.Value!.Id);
    }
}
