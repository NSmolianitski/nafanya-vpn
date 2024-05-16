using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram;

public class ReplyService : IReplyService
{
    private readonly ITelegramBotClient _botClient;

    private readonly string _helloMessage;
    
    public ReplyService(IConfiguration configuration,
        ITelegramBotClient botClient)
    {
        var subscriptionConfig = configuration.GetSection(SubscriptionConstants.Subscription);
        var subscriptionCost = subscriptionConfig[SubscriptionConstants.CostInRoubles];
        var subscriptionLength = subscriptionConfig[SubscriptionConstants.Length];

        _helloMessage = Resources.Strings.WelcomeMessage;
        _helloMessage = _helloMessage
            .Replace("%subscriptionCost%", $"{subscriptionCost}{PaymentConstants.CurrencySymbol}")
            .Replace("%subscriptionLength%", $"{subscriptionLength} дней");
        
        _botClient = botClient;
    }

    public async Task SendTextWithMainKeyboardAsync(long chatId, Subscription subscription, string text)
    {
        var replyMarkup = ReplyMarkups.CreateMainMarkup(subscription);
        await SendTextWithMarkupAsync(chatId, text, replyMarkup);
    }

    public async Task<Message> SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup)
    {
        return await _botClient.SendTextMessageAsync(chatId, text, replyMarkup: markup, 
            parseMode: ParseMode.Html);
    }

    public async Task EditMessageWithMarkupAsync(Message message, string newText, InlineKeyboardMarkup markup)
    {
        await _botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, newText, replyMarkup: markup, 
            parseMode: ParseMode.Html);
    }

    public async Task EditMessageAsync(Message message, string newText)
    {
        await _botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, newText, 
            parseMode: ParseMode.Html);
    }

    public async Task SendHelloAsync(long chatId, Subscription subscription)
    {
        await SendTextWithMainKeyboardAsync(chatId, subscription, _helloMessage);
    }

    public async Task SendChatActionAsync(ChatId chatId, ChatAction chatAction)
    {
        await _botClient.SendChatActionAsync(chatId, chatAction);
    }
}