namespace NafanyaVPN.Entities.PaymentNotifications;

public interface INotificationHandleService
{
    Task Handle(YoomoneyPaymentNotification notification);
}