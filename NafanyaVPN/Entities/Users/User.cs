using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;

namespace NafanyaVPN.Entities.Users;

public class User
{
    public User() {}
    
    public User(
        int id,
        DateTime createdAt,
        DateTime updatedAt,
        string telegramUserName,
        long telegramUserId,
        decimal moneyInRoubles,
        DateTime subscriptionEndDate,
        string telegramState,
        PaymentMessage? paymentMessage,
        OutlineKey? outlineKey,
        Subscription.Subscription subscription)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        TelegramUserName = telegramUserName;
        TelegramUserId = telegramUserId;
        MoneyInRoubles = moneyInRoubles;
        SubscriptionEndDate = subscriptionEndDate;
        TelegramState = telegramState;
        PaymentMessage = paymentMessage;
        OutlineKey = outlineKey;
        Subscription = subscription;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string TelegramUserName { get; set; }
    public long TelegramUserId { get; set; }
    public decimal MoneyInRoubles { get; set; }
    public DateTime SubscriptionEndDate { get; set; }
    public string TelegramState { get; set; }
    public PaymentMessage? PaymentMessage { get; set; }
    public OutlineKey? OutlineKey { get; set; }
    public Subscription.Subscription Subscription { get; set; }
}