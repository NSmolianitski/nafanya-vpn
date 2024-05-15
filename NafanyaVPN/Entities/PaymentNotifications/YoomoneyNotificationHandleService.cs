using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Telegram.Abstractions;

namespace NafanyaVPN.Entities.PaymentNotifications;

public class YoomoneyNotificationHandleService(
    IPaymentService paymentService,
    IUserService userService,
    IReplyService replyService,
    ISubscriptionRenewService subscriptionRenewService,
    ILogger<YoomoneyNotificationHandleService> logger)
    : INotificationHandleService
{
    public async Task Handle(YoomoneyPaymentNotification notification)
    {
        try
        {
            var payment = await paymentService.GetByLabelAsync(notification.Label!);
            if (payment.Status is PaymentStatusType.Canceled or PaymentStatusType.Finished)
                throw new BadPaymentNotificationException($"Статус Payment отличается от Waiting:\n{payment}");
            
            await paymentService.FinishPaymentAsync(payment);

            var user = await userService.GetByIdAsync(payment.UserId);
            await replyService.SendTextWithMainKeyboardAsync(user.TelegramChatId, user.Subscription,
                $"Счёт успешно пополнен на сумму {payment.Sum}{PaymentConstants.CurrencySymbol}!");
            
            if (!user.Subscription.RenewalDisabled)
                await subscriptionRenewService.RenewIfEnoughMoneyAsync(user);
        }
        catch (NoSuchEntityException e)
        {
            logger.LogWarning("{Message}", e);
        }
    }
}