namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionPlanRepository
{
    Task<SubscriptionPlan> CreateAsync(SubscriptionPlan model);
    Task<SubscriptionPlan> GetByNameAsync(string name);
    Task<SubscriptionPlan?> TryGetByNameAsync(string name);
    Task<bool> DeleteAsync(SubscriptionPlan model);
    Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan model);
    Task UpdateAllAsync(IEnumerable<SubscriptionPlan> models);
}