using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.PaymentNotifications;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Registration;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Telegram;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.UserInput;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using NafanyaVPN.Entities.Users;
using Serilog;
using Telegram.Bot;

namespace NafanyaVPN;

public static class AppBuilderExtensions
{
    public static void UseNafanyaVPNConfiguration(this WebApplicationBuilder appBuilder)
    {
        var settingsFilePath = appBuilder.Environment.IsDevelopment()
            ? AppSettingsPathConstants.Development
            : AppSettingsPathConstants.Production;
        appBuilder.Configuration.AddJsonFile(settingsFilePath);
    }
    
    public static void UseNafanyaVPNLogging(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Logging.ClearProviders();
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(appBuilder.Configuration)
            .WriteTo.SQLite(appBuilder.Environment.ContentRootPath + @"\Logs\logs.db");
        
        appBuilder.Logging.AddSerilog(loggerConfiguration.CreateLogger());
    }

    public static void UseNafanyaVPNTelegram(this WebApplicationBuilder appBuilder)
    {
        var telegramBotOptions = new TelegramBotClientOptions(
            appBuilder.Configuration[$"{TelegramConstants.SettingsSectionName}:{TelegramConstants.Token}"]!);
        var telegramBotClient = new TelegramBotClient(telegramBotOptions);
        appBuilder.Services.AddSingleton<ITelegramBotClient>(telegramBotClient);
    }

    public static void UseNafanyaVPNDatabase(this WebApplicationBuilder appBuilder)
    {
        var connectionString = appBuilder.Configuration.GetConnectionString(DatabaseConstants.Default);
        appBuilder.Services.AddDbContext<NafanyaVPNContext>(options => options.UseSqlite(connectionString));
    }
    
    public static void UseNafanyaVPNRepositories(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddScoped<IUserRepository, UserRepository>();
        appBuilder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        appBuilder.Services.AddScoped<IOutlineKeyRepository, OutlineKeyRepository>();
        appBuilder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanPlanRepository>();
        appBuilder.Services.AddScoped<IPaymentMessageRepository, PaymentMessageRepository>();
    }
    
    public static void UseNafanyaVPNServices(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddScoped<IUserService, UserService>();
        appBuilder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        appBuilder.Services.AddScoped<IOutlineKeyService, OutlineKeyService>();
        appBuilder.Services.AddScoped<IPaymentService, YoomoneyPaymentService>();
        appBuilder.Services.AddScoped<INotificationHandleService, YoomoneyNotificationHandleService>();
        appBuilder.Services.AddScoped<IPaymentMessageService, PaymentMessageService>();

        appBuilder.Services.AddScoped<ITelegramUpdatesHandlerService, TelegramUpdatesHandlerServiceService>();
        appBuilder.Services.AddScoped<ICommandHandlerService<MessageDto>, MessageCommandHandlerService>();
        appBuilder.Services.AddScoped<ICommandHandlerService<CallbackQueryDto>, CallbackCommandHandlerService>();

        appBuilder.Services.AddScoped<IReplyService, ReplyService>();
        appBuilder.Services.AddScoped<IOutlineService, OutlineService>();
        appBuilder.Services.AddScoped<ITelegramStateService, TelegramStateService>();
        appBuilder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
        appBuilder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
        appBuilder.Services.AddScoped<ISubscriptionDateTimeService, SubscriptionDateTimeService>();
        appBuilder.Services.AddScoped<ISubscriptionExtendService, SubscriptionExtendService>();
    }

    public static void UseNafanyaVPNCommands(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddScoped<SendAccountDataCommand>();
        appBuilder.Services.AddScoped<SendOutlineKeyCommand>();
        appBuilder.Services.AddScoped<SendInstructionCommand>();
        appBuilder.Services.AddScoped<SendHelloCommand>();
        appBuilder.Services.AddScoped<ConfirmPaymentSumCommand>();
        appBuilder.Services.AddScoped<CustomPaymentSumCommand>();
        appBuilder.Services.AddScoped<ConfirmCustomPaymentSumCommand>();
        appBuilder.Services.AddScoped<BackToPaymentSumCommand>();

        appBuilder.Services.AddScoped<SendPaymentSumChooseCommand>();

        appBuilder.Services.AddScoped<CheckCustomPaymentSumCommand>();
    }
}