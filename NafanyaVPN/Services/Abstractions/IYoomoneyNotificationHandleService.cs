using NafanyaVPN.Models;

namespace NafanyaVPN.Services;

public interface IYoomoneyNotificationHandleService
{
    void Handle(YoomoneyPaymentNotification notification);
}