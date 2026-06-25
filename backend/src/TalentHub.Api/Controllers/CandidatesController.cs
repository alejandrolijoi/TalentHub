using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.DTOs.Candidates;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public CandidatesController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet("me")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await _candidateService.GetByUserIdAsync(UserId);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateCandidateRequest request)
    {
        var result = await _candidateService.UpdateAsync(UserId, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("me/skills")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> UpdateMySkills([FromBody] UpdateCandidateSkillsRequest request)
    {
        var result = await _candidateService.UpdateSkillsAsync(UserId, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("me/resume")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> UploadResume(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        await using var stream = file.OpenReadStream();
        var result = await _candidateService.UploadResumeAsync(UserId, stream, file.FileName);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { url = result.Value });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublicProfile(Guid id)
    {
        var result = await _candidateService.GetPublicProfileAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }
}
