namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionPlanService
{
    Task<SubscriptionPlan> GetByNameAsync(string name);
    Task UpdateAsync(SubscriptionPlan model);
}