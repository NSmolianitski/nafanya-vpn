using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.PaymentNotifications;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Registration;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Entities.Withdraws;
using NafanyaVPN.Telegram;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.CommandHandlers;
using NafanyaVPN.Telegram.Commands.Callbacks;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.Commands.UserInput;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;
using Serilog;
using Telegram.Bot;
using TelegramSink;

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
        var configuration = appBuilder.Configuration;
        var telegramSection = configuration.GetRequiredSection(TelegramConstants.SettingsSectionName);
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.TeleSink(
                telegramSection[TelegramConstants.LogBotToken], 
                telegramSection[TelegramConstants.LogBotChatId])
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
        appBuilder.Services.AddScoped<IPaymentMessageRepository, PaymentMessageRepository>();
        appBuilder.Services.AddScoped<IWithdrawRepository, WithdrawRepository>();
        appBuilder.Services.AddScoped<IOutlineKeyRepository, OutlineKeyRepository>();
        appBuilder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        appBuilder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
    }
    
    public static void UseNafanyaVPNServices(this WebApplicationBuilder appBuilder)
    {
        // Основные сервисы
        appBuilder.Services.AddScoped<IUserService, UserService>();
        appBuilder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
        appBuilder.Services.AddScoped<IPaymentService, YoomoneyPaymentService>();
        appBuilder.Services.AddScoped<IOutlineKeyService, OutlineKeyService>();
        appBuilder.Services.AddScoped<IWithdrawService, WithdrawService>();
        appBuilder.Services.AddScoped<INotificationHandleService, YoomoneyNotificationHandleService>();
        appBuilder.Services.AddScoped<IPaymentMessageService, PaymentMessageService>();
        appBuilder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
        appBuilder.Services.AddScoped<IOutlineService, OutlineService>();
        appBuilder.Services.AddScoped<IReplyService, ReplyService>();
        appBuilder.Services.AddScoped<ITelegramStateService, TelegramStateService>();
        appBuilder.Services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
        appBuilder.Services.AddScoped<ISubscriptionDateTimeService, SubscriptionDateTimeService>();
        appBuilder.Services.AddScoped<ISubscriptionRenewService, SubscriptionRenewService>();

        // Обработка команд Telegram
        appBuilder.Services.AddScoped<ITelegramUpdatesHandlerService, TelegramUpdatesHandlerService>();
        appBuilder.Services.AddScoped<ICommandHandlerService<MessageDto>, MessageCommandHandlerService>();
        appBuilder.Services.AddScoped<ICommandHandlerService<CallbackQueryDto>, CallbackCommandHandlerService>();
    }

    public static void UseNafanyaVPNCommands(this WebApplicationBuilder appBuilder)
    {
        // Основное меню
        appBuilder.Services.AddScoped<HelloCommand>();
        appBuilder.Services.AddScoped<AccountDataCommand>();
        appBuilder.Services.AddScoped<OutlineKeyCommand>();
        appBuilder.Services.AddScoped<InstructionCommand>();
        appBuilder.Services.AddScoped<SettingsCommand>();
        appBuilder.Services.AddScoped<RenewSubscriptionCommand>();
        // Меню настроек
        appBuilder.Services.AddScoped<ToggleRenewalCommand>();
        appBuilder.Services.AddScoped<ToggleRenewalNotificationsCommand>();
        appBuilder.Services.AddScoped<ToggleSubEndNotificationsCommand>();
        appBuilder.Services.AddScoped<BackToMainMenuCommand>();
        
        // Меню оплаты
        appBuilder.Services.AddScoped<ConfirmPaymentSumCommand>();
        appBuilder.Services.AddScoped<CustomPaymentSumCommand>();
        appBuilder.Services.AddScoped<ConfirmCustomPaymentSumCommand>();
        appBuilder.Services.AddScoped<BackToPaymentSumCommand>();

        // Текстовый ввод суммы (отключён)
        appBuilder.Services.AddScoped<PaymentSumChooseCommand>();
        appBuilder.Services.AddScoped<CheckCustomPaymentSumCommand>();
    }
}