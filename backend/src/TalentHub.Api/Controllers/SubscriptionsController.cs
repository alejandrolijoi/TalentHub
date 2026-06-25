using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.DTOs.Plans;
using TalentHub.Application.DTOs.Subscriptions;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ICompanyService _companyService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public SubscriptionsController(ISubscriptionService subscriptionService, ICompanyService companyService)
    {
        _subscriptionService = subscriptionService;
        _companyService = companyService;
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlans()
    {
        var result = await _subscriptionService.GetPlansAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("plans/{id}")]
    public async Task<IActionResult> GetPlan(Guid id)
    {
        var result = await _subscriptionService.GetPlanByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("current")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetCurrentSubscription()
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.GetCurrentSubscriptionAsync(companyId.Value);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("checkout")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.CreateCheckoutSessionAsync(companyId.Value, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("change-plan")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> ChangePlan([FromBody] ChangePlanRequest request)
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.ChangePlanAsync(companyId.Value, request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { message = "Plan changed successfully" });
    }

    [HttpPost("cancel")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> CancelSubscription()
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.CancelSubscriptionAsync(companyId.Value);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { message = "Subscription will be canceled at the end of the billing period" });
    }

    [HttpGet("billing/portal")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetBillingPortal()
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.GetBillingPortalUrlAsync(companyId.Value);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("billing/invoices")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetInvoices([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.GetInvoicesAsync(companyId.Value, page, pageSize);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("usage")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> GetUsage()
    {
        var companyId = await GetCompanyId();
        if (companyId == null) return BadRequest("Company not found");

        var result = await _subscriptionService.GetCurrentMonthUsageAsync(companyId.Value);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { usage = result.Value });
    }

    private async Task<Guid?> GetCompanyId()
    {
        var result = await _companyService.GetByUserIdAsync(UserId);
        return result.Value?.Id;
    }
}
