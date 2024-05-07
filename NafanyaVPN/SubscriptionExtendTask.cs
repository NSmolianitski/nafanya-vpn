using System.Globalization;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Utils;

namespace NafanyaVPN;

public class SubscriptionExtendTask(IServiceScopeFactory scopeFactory, ILogger<SubscriptionExtendTask> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dateTimeService = scope.ServiceProvider.GetRequiredService<ISubscriptionDateTimeService>();
                var subscriptionExtendService = scope.ServiceProvider.GetRequiredService<ISubscriptionExtendService>();

                await subscriptionExtendService.ExtendForAllUsers();

                var nextUpdateDelay = dateTimeService.GetDelayForNextSubscriptionUpdate();
                logger.LogInformation("Следующее обновление подписки: {Datetime}",
                    (DateTimeUtils.GetMoscowTime() + nextUpdateDelay)
                    .ToString(CultureInfo.InvariantCulture));
            
                await Task.Delay(nextUpdateDelay, stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError("{Message}", e.Message);
            }
        }
    }
}