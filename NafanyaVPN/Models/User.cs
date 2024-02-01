namespace NafanyaVPN.Models;

public class User
{
    public int Id { get; set; }
    public string TelegramUserName { get; set; }
    public long TelegramUserId { get; set; }
    public decimal MoneyInRoubles { get; set; }
    public DateTime SubscriptionEndDate { get; set; }
    public string TelegramState { get; set; }
    public virtual OutlineKey? OutlineKey { get; set; }
    public virtual Subscription Subscription { get; set; }
}