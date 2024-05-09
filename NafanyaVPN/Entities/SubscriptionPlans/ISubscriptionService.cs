namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionService
{
    Task<SubscriptionPlan> GetAsync(string name);
    Task UpdateAsync(SubscriptionPlan model);
}