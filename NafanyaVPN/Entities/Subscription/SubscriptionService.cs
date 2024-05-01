using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;

namespace NafanyaVPN.Entities.Subscription;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public async Task<Subscription> GetAsync(string name = DatabaseConstants.Default)
    {
        var subscription = await subscriptionRepository.GetAll()
            .FirstOrDefaultAsync(s => s.Name == name);

        return subscription!;
    }

    public async Task UpdateAsync(Subscription model)
    {
        await subscriptionRepository.UpdateAsync(model);
    }
}