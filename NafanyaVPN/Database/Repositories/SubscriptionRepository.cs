using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;

namespace NafanyaVPN.Database.Repositories;

public class SubscriptionRepository : IBaseRepository<Subscription>
{
    private readonly NafanyaVPNContext _db;

    public SubscriptionRepository(NafanyaVPNContext db)
    {
        _db = db;
    }
    
    public async Task<Subscription> CreateAsync(Subscription model)
    {
        var subscription = await _db.Subscriptions.AddAsync(model);
        await _db.SaveChangesAsync();
        return subscription.Entity;
    }

    public IQueryable<Subscription> GetAll()
    {
        return _db.Subscriptions;
    }

    public async Task<bool> DeleteAsync(Subscription model)
    {
        _db.Subscriptions.Remove(model);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Subscription> UpdateAsync(Subscription model)
    {
        _db.Subscriptions.Update(model);
        await _db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<Subscription> models)
    {
        foreach (var model in models)
        {
            _db.Subscriptions.Update(model);
        }
        await _db.SaveChangesAsync();
    }
}