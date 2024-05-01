using NafanyaVPN.Database;

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
        db.Subscriptions.Update(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<Subscription> models)
    {
        foreach (var model in models)
        {
            db.Subscriptions.Update(model);
        }
        await db.SaveChangesAsync();
    }
}