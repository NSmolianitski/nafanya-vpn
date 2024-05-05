using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.PaymentNotifications;

public class YoomoneyNotificationHandleService(
    IPaymentService paymentService, 
    ILogger<YoomoneyNotificationHandleService> logger)
    : INotificationHandleService
{
    public async Task Handle(YoomoneyPaymentNotification notification)
    {
        try
        {
            var payment = await paymentService.GetByLabelAsync(notification.Label);
            await paymentService.FinishPaymentAsync(payment);
        }
        catch (NoSuchEntityException e)
        {
            logger.LogWarning(e.Message);
        }
    }
}