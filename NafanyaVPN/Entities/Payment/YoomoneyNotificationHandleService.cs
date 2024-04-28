using NafanyaVPN.Entities.MoneyOperations;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Payment;

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