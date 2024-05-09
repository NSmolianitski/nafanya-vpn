namespace NafanyaVPN.Entities.Subscriptions;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public async Task<Subscription> CreateAsync(Subscription model)
    {
        return await subscriptionRepository.CreateAsync(model);
    }

    public async Task<List<Subscription>> GetAllAsync()
    {
        return await subscriptionRepository.GetAllAsync();
    }
    
    public async Task<List<Subscription>> GetAllNonExpiredAsync()
    {
        return await subscriptionRepository.GetAllNonExpiredAsync();
    }

    public async Task<Subscription> UpdateAsync(Subscription model)
    {
        return await subscriptionRepository.UpdateAsync(model);
    }

    public async Task UpdateAllAsync(IEnumerable<Subscription> models)
    {
        await subscriptionRepository.UpdateAllAsync(models);
    }
}