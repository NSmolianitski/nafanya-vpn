using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Subscriptions;

public class SubscriptionBuilder
{
    private int _id;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private User _user;
    private DateTime _endDate;
    private int _subscriptionPlanId;
    private SubscriptionPlan _subscriptionPlan;
    private bool _renewalDisabled;
    private bool _hasExpired;
    
    public SubscriptionBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public SubscriptionBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }
    
    public SubscriptionBuilder WithNowCreatedAt()
    {
        _createdAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }

    public SubscriptionBuilder WithNowCreatedAtUpdatedAt()
    {
        var nowTime = DateTimeUtils.GetMoscowNowTime();
        _createdAt = nowTime;
        _updatedAt = nowTime;
        return this;
    }
    
    public SubscriptionBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }
    
    public SubscriptionBuilder WithNowUpdatedAt()
    {
        _updatedAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }
    
    public SubscriptionBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }
    
    public SubscriptionBuilder WithEndDate(DateTime endDate)
    {
        _endDate = endDate;
        return this;
    }
    
    public SubscriptionBuilder WithSubscriptionPlanId(int subscriptionPlanId)
    {
        _subscriptionPlanId = subscriptionPlanId;
        return this;
    }

    public SubscriptionBuilder WithSubscriptionPlan(SubscriptionPlan subscriptionPlan)
    {
        _subscriptionPlanId = subscriptionPlan.Id;
        _subscriptionPlan = subscriptionPlan;
        return this;
    }
    
    public SubscriptionBuilder WithRenewalDisabled(bool renewalDisabled)
    {
        _renewalDisabled = renewalDisabled;
        return this;
    }
    
    public SubscriptionBuilder WithHasExpired(bool hasExpired)
    {
        _hasExpired = hasExpired;
        return this;
    }

    public Subscription Build() => new Subscription(
        _id,
        _createdAt,
        _updatedAt,
        _user,
        _endDate,
        _subscriptionPlanId,
        _subscriptionPlan,
        _renewalDisabled,
        _hasExpired
    );
}