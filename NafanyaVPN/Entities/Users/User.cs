using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;

namespace NafanyaVPN.Entities.Users;

public class User
{
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