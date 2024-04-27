using Microsoft.AspNetCore.Mvc;
using NafanyaVPN.Models;
using NafanyaVPN.Services;

namespace NafanyaVPN.Controllers;

[Route("api/v1/")]
public class YoomoneyNotificationController(
    ILogger<YoomoneyNotificationController> logger,
    IModelValidationLogBuilderService logBuilderService) : Controller
{
    [HttpPost]
    [Route("payment-notification")]
    public async Task<IActionResult> NotifyAboutPayment([FromForm] YoomoneyPaymentNotification notification)
    {
        var modelLog = logBuilderService.GetModelValidationLog(ModelState);
        
        if (!ModelState.IsValid)
        {
            logger.LogError(modelLog);
            return BadRequest();
        }
        
        logger.LogInformation(modelLog);
        return Ok();
    }
}