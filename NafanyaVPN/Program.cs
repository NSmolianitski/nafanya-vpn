using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NafanyaVPN;
using NafanyaVPN.Database;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Registration;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Telegram;
using NafanyaVPN.Entities.Telegram.CommandHandlers;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.UserInput;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

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

var settingFilePath = appBuilder.Environment.IsDevelopment()
    ? AppSettingsPathConstants.Development
    : AppSettingsPathConstants.Production;
var configuration = appBuilder.Configuration;
configuration.AddJsonFile(settingFilePath);

appBuilder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(configuration)
    .WriteTo.SQLite(appBuilder.Environment.ContentRootPath + @"\Logs\logs.db");

appBuilder.Logging.AddSerilog(loggerConfiguration.CreateLogger());

var telegramBotOptions = new TelegramBotClientOptions(
    configuration[$"{TelegramConstants.SettingsSectionName}:{TelegramConstants.Token}"]!);
var telegramBotClient = new TelegramBotClient(telegramBotOptions);
appBuilder.Services.AddSingleton<ITelegramBotClient>(telegramBotClient);

// DATABASE
var connectionString = configuration.GetConnectionString(DatabaseConstants.Default);
appBuilder.Services.AddDbContext<NafanyaVPNContext>(options => options.UseSqlite(connectionString));

// REPOSITORIES
appBuilder.Services.AddScoped<IUserRepository, UserRepository>();
appBuilder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
appBuilder.Services.AddScoped<IOutlineKeyRepository, OutlineKeyRepository>();
appBuilder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// SERVICES
appBuilder.Services.AddScoped<IUserService, UserService>();
appBuilder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
appBuilder.Services.AddScoped<IOutlineKeysService, OutlineKeysService>();
appBuilder.Services.AddScoped<IPaymentService, YoomoneyPaymentService>();

appBuilder.Services.AddScoped<IUpdateHandler, MessageReceiveService>();
appBuilder.Services.AddScoped<ICommandHandlerService<Message>, MessageCommandHandlerService>();
appBuilder.Services.AddScoped<ICommandHandlerService<CallbackQueryDto>, CallbackCommandHandlerService>();

appBuilder.Services.AddScoped<IReplyService, ReplyService>();
appBuilder.Services.AddScoped<IOutlineService, OutlineService>();
appBuilder.Services.AddScoped<ITelegramStateService, TelegramStateService>();
appBuilder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
appBuilder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
appBuilder.Services.AddScoped<ISubscriptionDateTimeService, SubscriptionDateTimeService>();
appBuilder.Services.AddScoped<ISubscriptionExtendService, SubscriptionExtendService>();

// COMMANDS
appBuilder.Services.AddScoped<SendAccountDataCommand>();
appBuilder.Services.AddScoped<DonateMoneyCommand>();
appBuilder.Services.AddScoped<SendOutlineKeyCommand>();
appBuilder.Services.AddScoped<SendInstructionCommand>();
appBuilder.Services.AddScoped<SendHelloCommand>();
appBuilder.Services.AddScoped<PaymentSumCommand>();
appBuilder.Services.AddScoped<CustomPaymentSumCommand>();

appBuilder.Services.AddScoped<SendPaymentSumChooseCommand>();

appBuilder.Services.AddScoped<CheckCustomPaymentSumCommand>();

// APP LAUNCH
appBuilder.Services.AddHostedService<BotInitTask>();
// appBuilder.Services.AddHostedService<SubscriptionExtendTask>();

var app = appBuilder.Build();

// app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

await app.RunAsync();