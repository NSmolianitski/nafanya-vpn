﻿using Microsoft.AspNetCore.Mvc;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Payment;

[Route("api/v1/")]
public class YoomoneyNotificationController(
    ILogger<YoomoneyNotificationController> logger,
    INotificationHandleService notificationHandleService) : Controller
{
    [HttpPost]
    [Route("payment-notification")]
    public async Task<IActionResult> NotifyAboutPayment([FromForm] YoomoneyPaymentNotification notification)
    {
        var modelLog = ModelValidationLogBuilderBuilderUtils.GetModelValidationLog(ModelState);
        
        if (!ModelState.IsValid)
        {
            logger.LogError(modelLog);
            return BadRequest();
        }
        
        logger.LogInformation(modelLog);
        await notificationHandleService.Handle(notification);
        
        return Ok();
    }
}