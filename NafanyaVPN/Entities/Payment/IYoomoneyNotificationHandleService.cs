namespace NafanyaVPN.Entities.Payment;

public interface IYoomoneyNotificationHandleService
{
    void Handle(YoomoneyPaymentNotification notification);
}