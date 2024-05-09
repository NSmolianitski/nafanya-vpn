using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionPlanRepository(NafanyaVPNContext db) : ISubscriptionPlanRepository
{
    public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan model)
    {
        var subscription = await db.Subscriptions.AddAsync(model);
        await db.SaveChangesAsync();
        return subscription.Entity;
    }

    public IQueryable<SubscriptionPlan> GetAll()
    {
        return db.Subscriptions;
    }

    public async Task<bool> DeleteAsync(SubscriptionPlan model)
    {
        db.Subscriptions.Remove(model);
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
        return db.Subscriptions.Update(model);
    }
}