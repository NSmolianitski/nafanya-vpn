using Microsoft.AspNetCore.Mvc;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.PaymentNotifications;

[Route("api/v1/")]
public class PaymentNotificationController(
    ILogger<PaymentNotificationController> logger,
    INotificationHandleService notificationHandleService) : Controller
{
    [HttpPost]
    [Route("payment-notification")]
    public async Task<IActionResult> NotifyAboutPayment([FromForm] YoomoneyPaymentNotification notification)
    {
        var modelLog = ModelValidationLogBuilderBuilderUtils.GetModelValidationLog(ModelState);
        
        if (!ModelState.IsValid)
        {
            logger.LogError("{ModelLog}", modelLog);
            return BadRequest();
        }
        
        logger.LogInformation("{ModelLog}", modelLog);
        await notificationHandleService.Handle(notification);
        
        return Ok();
    }
}