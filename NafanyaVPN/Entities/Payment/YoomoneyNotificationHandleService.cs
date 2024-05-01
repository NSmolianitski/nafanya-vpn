using NafanyaVPN.Entities.MoneyOperations;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Payment;

public class YoomoneyNotificationHandleService(
    IMoneyOperationService moneyOperationService, 
    ILogger<YoomoneyNotificationHandleService> logger)
    : INotificationHandleService
{
    public async Task Handle(YoomoneyPaymentNotification notification)
    {
        try
        {
            var moneyOperation = await moneyOperationService.GetByLabelAsync(notification.Label);
        }
        catch (NoSuchEntityException e)
        {
            logger.LogWarning(e.Message);
        }
    }
}