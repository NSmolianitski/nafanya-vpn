namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionPlanService
{
    Task<SubscriptionPlan> GetAsync(string name);
    Task UpdateAsync(SubscriptionPlan model);
}