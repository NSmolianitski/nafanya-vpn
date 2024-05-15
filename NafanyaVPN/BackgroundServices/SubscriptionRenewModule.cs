using System.Globalization;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Utils;

namespace NafanyaVPN;

public class SubscriptionRenewModule(
    IServiceScopeFactory scopeFactory,
    ILogger<SubscriptionRenewModule> logger)
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
                var subscriptionExtendService = scope.ServiceProvider.GetRequiredService<ISubscriptionRenewService>();

                var nextUpdateDelay = dateTimeService.GetDelayBetweenChecks();
                logger.LogInformation("Следующее обновление подписки: {Datetime}",
                    (DateTimeUtils.GetMoscowNowTime() + nextUpdateDelay)
                    .ToString(CultureInfo.InvariantCulture));
                
                await subscriptionExtendService.RenewAllNonExpiredAsync();
            
                await Task.Delay(nextUpdateDelay, stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError("{Message}", e.Message);
            }
        }
    }
}