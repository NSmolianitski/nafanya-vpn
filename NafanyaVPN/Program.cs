using System.Globalization;
using NafanyaVPN;
using NafanyaVPN.Database;

var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Services.AddControllers();

var cultureInfo = new CultureInfo("ru-RU")
{
    NumberFormat =
    {
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ","
    }
};
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

appBuilder.UseNafanyaVPNConfiguration();
appBuilder.UseNafanyaVPNLogging();
appBuilder.UseNafanyaVPNTelegram();
appBuilder.UseNafanyaVPNDatabase();
appBuilder.UseNafanyaVPNRepositories();
appBuilder.UseNafanyaVPNServices();
appBuilder.UseNafanyaVPNCommands();

// APP LAUNCH
appBuilder.Services.AddHostedService<BotInitTask>();
appBuilder.Services.AddHostedService<SubscriptionExtendTask>();

var app = appBuilder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NafanyaVPNContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

await app.RunAsync();