using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Constants;
using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class SubscriptionService(IBaseRepository<Subscription> subscriptionRepository) : ISubscriptionService
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