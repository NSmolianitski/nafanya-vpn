using NafanyaVPN.Constants;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class SubscriptionExtendService : ISubscriptionExtendService
{
    private readonly IUserService _userService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IOutlineService _outlineService;
    private readonly ISubscriptionDateTimeService _dateTimeService;
    private readonly ILogger<SubscriptionExtendService> _logger;

    public SubscriptionExtendService(
        IUserService userService, 
        ISubscriptionService subscriptionService, 
        IOutlineService outlineService, 
        ISubscriptionDateTimeService dateTimeService, 
        ILogger<SubscriptionExtendService> logger)
    {
        _userService = userService;
        _subscriptionService = subscriptionService;
        _outlineService = outlineService;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task ExtendForAllUsers()
    {
        DateTime newSubscriptionEndDate = _dateTimeService.GetNewSubscriptionEndDate();
        
        Subscription defaultSubscription = await _subscriptionService.GetAsync(DatabaseConstants.Default);

        if (defaultSubscription.NextUpdateTime > _dateTimeService.Now())
        {
            _logger.LogInformation($"Стандартная подписка пока не может обновиться. " +
                                   $"Это не ошибка (может появляться при запуске приложения). " +
                                   $"Следующее обновление: {defaultSubscription.NextUpdateTime}");
            return;
        }

        defaultSubscription.NextUpdateTime = newSubscriptionEndDate;
        await _subscriptionService.UpdateAsync(defaultSubscription);
        
        var users = await _userService.GetAllAsync();
        foreach (var user in users)
        {
            var subscriptionPrice = user.Subscription.DailyCostInRoubles;
            if (subscriptionPrice > user.MoneyInRoubles)
            {
                if (user.OutlineKey is null)
                    continue;
                
                var keyId = user.OutlineKey!.Id;
                _outlineService.DisableKey(keyId);
                _logger.LogInformation($"Ключ деактивирован. " +
                                       $"На счёте {user.TelegramUserName}({user.TelegramUserId}) " +
                                       $"недостаточно средств. Стоимость подписки: {subscriptionPrice}. " +
                                       $"Текущий баланс: {user.MoneyInRoubles}");
            }
            else
            {
                user.MoneyInRoubles -= subscriptionPrice;
                user.SubscriptionEndDate = newSubscriptionEndDate;
                _logger.LogInformation($"Со счёта {user.TelegramUserName}({user.TelegramUserId}) " +
                                       $"списано {subscriptionPrice} рублей. Осталось: {user.MoneyInRoubles}");
            }
        }

        await _userService.UpdateAllAsync(users);
    }

    public async Task TryExtendForUser(User user)
    {
        var subscriptionPrice = user.Subscription.DailyCostInRoubles;
        if (subscriptionPrice > user.MoneyInRoubles)
        {
            var keyId = user.OutlineKey!.Id;
            _outlineService.DisableKey(keyId);
        }
        else
        {
            user.MoneyInRoubles -= subscriptionPrice;
            user.SubscriptionEndDate = user.Subscription.NextUpdateTime;
        }

        await _userService.UpdateAsync(user);
    }
}