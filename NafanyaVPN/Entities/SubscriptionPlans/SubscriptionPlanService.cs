using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository) : ISubscriptionPlanService
{
    public async Task<SubscriptionPlan> GetAsync(string name = DatabaseConstants.Default)
    {
        var subscription = await subscriptionPlanRepository.GetAll()
            .FirstOrDefaultAsync(s => s.Name == name);

        return subscription!;
    }

    public async Task UpdateAsync(SubscriptionPlan model)
    {
        await subscriptionPlanRepository.UpdateAsync(model);
    }
}