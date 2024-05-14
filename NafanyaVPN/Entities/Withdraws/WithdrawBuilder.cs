using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Withdraws;

public class WithdrawBuilder
{
    private long _id;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private User _user;
    private SubscriptionPlan _subscriptionPlan;
    private decimal _sum;
    
    public WithdrawBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public WithdrawBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public WithdrawBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }
    
    public WithdrawBuilder WithNowCreatedAt()
    {
        _createdAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }
    public WithdrawBuilder WithNowCreatedAtUpdatedAt()
    {
        var nowTime = DateTimeUtils.GetMoscowNowTime();
        _createdAt = nowTime;
        _updatedAt = nowTime;
        return this;
    }
    public WithdrawBuilder WithNowUpdatedAt()
    {
        _updatedAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }

    public WithdrawBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }

    public WithdrawBuilder WithSubscriptionPlan(SubscriptionPlan subscriptionPlan)
    {
        _subscriptionPlan = subscriptionPlan;
        return this;
    }

    public WithdrawBuilder WithSum(decimal sum)
    {
        _sum = sum;
        return this;
    }

    public Withdraw Build() => new Withdraw(_id, _createdAt, _updatedAt, _user, _subscriptionPlan, _sum);
}