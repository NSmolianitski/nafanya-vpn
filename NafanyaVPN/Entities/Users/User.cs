using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.SubscriptionPlans;

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
        SubscriptionPlan subscriptionPlan)
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
        SubscriptionPlan = subscriptionPlan;
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
    public SubscriptionPlan SubscriptionPlan { get; set; }
}