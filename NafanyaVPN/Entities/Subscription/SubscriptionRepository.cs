using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Subscription;

public class SubscriptionRepository(NafanyaVPNContext db) : ISubscriptionRepository
{
    public async Task<Subscription> CreateAsync(Subscription model)
    {
        var subscription = await db.Subscriptions.AddAsync(model);
        await db.SaveChangesAsync();
        return subscription.Entity;
    }

    public IQueryable<Subscription> GetAll()
    {
        return db.Subscriptions;
    }

    public async Task<bool> DeleteAsync(Subscription model)
    {
        db.Subscriptions.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<Subscription> UpdateAsync(Subscription model)
    {
        UpdateWithoutSaving(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<Subscription> models)
    {
        foreach (var model in models)
        {
            UpdateWithoutSaving(model);
        }
        await db.SaveChangesAsync();
    }
    
    private EntityEntry<Subscription> UpdateWithoutSaving(Subscription model)
    {
        model.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
        return db.Subscriptions.Update(model);
    }
}