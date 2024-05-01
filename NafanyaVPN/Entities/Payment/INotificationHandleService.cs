namespace NafanyaVPN.Entities.Payment;

public interface INotificationHandleService
{
    Task Handle(YoomoneyPaymentNotification notification);
}