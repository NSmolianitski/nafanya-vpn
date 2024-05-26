using NafanyaVPN.Entities.Subscriptions;

namespace NafanyaVPN.Entities.Withdraws;

public interface IWithdrawService
{
    Withdraw CreateWithoutSaving(Subscription subscription);
}

