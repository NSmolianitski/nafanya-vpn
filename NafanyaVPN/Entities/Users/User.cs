using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;

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
        long telegramChatId,
        decimal moneyInRoubles,
        string telegramState,
        PaymentMessage? paymentMessage,
        OutlineKey? outlineKey,
        Subscription subscription)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        TelegramUserName = telegramUserName;
        TelegramUserId = telegramUserId;
        TelegramChatId = telegramChatId;
        MoneyInRoubles = moneyInRoubles;
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
    public long TelegramChatId { get; set; }
    public decimal MoneyInRoubles { get; set; }
    public string TelegramState { get; set; }
    public PaymentMessage? PaymentMessage { get; set; }
    public OutlineKey? OutlineKey { get; set; }
    public Subscription Subscription { get; set; }
}