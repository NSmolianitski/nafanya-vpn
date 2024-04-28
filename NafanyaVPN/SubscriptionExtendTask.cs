using System.Globalization;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Utils;

namespace NafanyaVPN;

public class SubscriptionExtendTask : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SubscriptionExtendTask> _logger;

    public SubscriptionExtendTask(IServiceScopeFactory scopeFactory, ILogger<SubscriptionExtendTask> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dateTimeService = scope.ServiceProvider.GetRequiredService<ISubscriptionDateTimeService>();
                var subscriptionExtendService = scope.ServiceProvider.GetRequiredService<ISubscriptionExtendService>();

                await subscriptionExtendService.ExtendForAllUsers();

                var nextUpdateDelay = dateTimeService.GetDelayForNextSubscriptionUpdate();
                _logger.LogInformation("Следующее обновление подписки: " 
                                       + (DateTimeUtils.GetMoscowTime() + nextUpdateDelay)
                                       .ToString(CultureInfo.InvariantCulture));
            
                await Task.Delay(nextUpdateDelay, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}