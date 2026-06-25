using Microsoft.AspNetCore.Mvc;
using TalentHub.Application.Services;

namespace TalentHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhooksController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public WebhooksController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost("stripe")]
    public async Task<IActionResult> StripeWebhook()
    {
        string json;
        using (var reader = new StreamReader(HttpContext.Request.Body))
        {
            json = await reader.ReadToEndAsync();
        }

        var signature = Request.Headers["Stripe-Signature"].ToString();
        var result = await _subscriptionService.HandleStripeWebhookAsync(json, signature);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpPost("mercadopago")]
    public async Task<IActionResult> MercadoPagoWebhook()
    {
        string json;
        using (var reader = new StreamReader(HttpContext.Request.Body))
        {
            json = await reader.ReadToEndAsync();
        }

        return Ok();
    }
}
