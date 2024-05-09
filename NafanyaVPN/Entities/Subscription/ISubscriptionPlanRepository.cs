namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionPlanRepository
{
    Task<SubscriptionPlan> CreateAsync(SubscriptionPlan model);
    IQueryable<SubscriptionPlan> GetAll();
    Task<bool> DeleteAsync(SubscriptionPlan model);
    Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan model);
    Task UpdateAllAsync(IEnumerable<SubscriptionPlan> models);
}