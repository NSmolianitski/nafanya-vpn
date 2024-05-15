using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Subscriptions;

/// <summary>
/// Подписка одного конкретного пользователя. Имеет ссылку на план подписки с общей информацией
/// </summary>
public class Subscription
{
    public Subscription() {}
    
    public Subscription(int id,
        DateTime createdAt,
        DateTime updatedAt,
        User user,
        DateTime endDateTime,
        int subscriptionPlanId,
        SubscriptionPlan subscriptionPlan,
        bool renewalDisabled,
        bool hasExpired,
        bool endNotificationsDisabled,
        bool renewalNotificationsDisabled,
        bool endNotificationPerformed)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        User = user;
        EndDateTime = endDateTime;
        SubscriptionPlanId = subscriptionPlanId;
        SubscriptionPlan = subscriptionPlan;
        RenewalDisabled = renewalDisabled;
        HasExpired = hasExpired;
        EndNotificationsDisabled = endNotificationsDisabled;
        RenewalNotificationsDisabled = renewalNotificationsDisabled;
        EndNotificationPerformed = endNotificationPerformed;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime EndDateTime { get; set; }
    public int SubscriptionPlanId { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public bool RenewalDisabled { get; set; }
    public bool HasExpired { get; set; }
    public bool EndNotificationsDisabled { get; set; }
    public bool RenewalNotificationsDisabled { get; set; }
    public bool EndNotificationPerformed { get; set; }
}