namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository) : ISubscriptionPlanService
{
    public async Task<SubscriptionPlan> GetByNameAsync(string name)
    {
        return await subscriptionPlanRepository.GetByNameAsync(name);
    }

    public async Task UpdateAsync(SubscriptionPlan model)
    {
        await subscriptionPlanRepository.UpdateAsync(model);
    }
}