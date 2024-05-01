using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.PaymentNotifications;

public class YoomoneyNotificationHandleService(
    Payments.IPaymentService paymentService, 
    ILogger<YoomoneyNotificationHandleService> logger)
    : INotificationHandleService
{
    public async Task Handle(YoomoneyPaymentNotification notification)
    {
        try
        {
            var payment = await paymentService.GetByLabelAsync(notification.Label);
        }
        catch (NoSuchEntityException e)
        {
            logger.LogWarning(e.Message);
        }
    }
}