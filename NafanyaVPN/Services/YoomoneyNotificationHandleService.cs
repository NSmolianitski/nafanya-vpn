using NafanyaVPN.Exceptions;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class YoomoneyNotificationHandleService(
    IMoneyOperationService moneyOperationService, 
    ILogger<YoomoneyNotificationHandleService> logger)
    : IYoomoneyNotificationHandleService
{
    public void Handle(YoomoneyPaymentNotification notification)
    {
        try
        {
            var moneyOperation = moneyOperationService.GetByLabelAsync(notification.Label);
        }
        catch (NoSuchEntityException e)
        {
            logger.LogWarning(e.Message);
        }
    }
}