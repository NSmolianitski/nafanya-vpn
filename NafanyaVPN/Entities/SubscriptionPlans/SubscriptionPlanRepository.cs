using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionPlanRepository(NafanyaVPNContext db) : ISubscriptionPlanRepository
{
    private IQueryable<SubscriptionPlan> Items => db.SubscriptionPlans;
    
    public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan model)
    {
        var subscription = await db.SubscriptionPlans.AddAsync(model);
        await db.SaveChangesAsync();
        return subscription.Entity;
    }

    public async Task<SubscriptionPlan> GetByNameAsync(string name)
    {
        var subscriptionPlan = await TryGetByNameAsync(name) ?? 
                               throw new NoSuchEntityException(
                                   $"Subscription plan with name: \"{name}\" does not exist. " + 
                                   $"Repository: \"{GetType().Name}\".");

        return subscriptionPlan;
    }
    
    public async Task<SubscriptionPlan?> TryGetByNameAsync(string name)
    {
        return await Items.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<bool> DeleteAsync(SubscriptionPlan model)
    {
        db.SubscriptionPlans.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan model)
    {
        UpdateWithoutSaving(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<SubscriptionPlan> models)
    {
        foreach (var model in models)
        {
            UpdateWithoutSaving(model);
        }
        await db.SaveChangesAsync();
    }
    
    private EntityEntry<SubscriptionPlan> UpdateWithoutSaving(SubscriptionPlan model)
    {
        model.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
        return db.SubscriptionPlans.Update(model);
    }
}