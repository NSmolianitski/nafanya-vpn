using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Constants;
using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IBaseRepository<Subscription> _subscriptionRepository;

    public SubscriptionService(IBaseRepository<Subscription> subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Subscription> GetAsync(string name = DatabaseConstants.Default)
    {
        var subscription = await _subscriptionRepository.GetAll()
            .FirstOrDefaultAsync(s => s.Name == name);

        return subscription!;
    }

    public async Task UpdateAsync(Subscription model)
    {
        await _subscriptionRepository.UpdateAsync(model);
    }
}