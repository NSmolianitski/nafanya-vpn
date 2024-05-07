using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = NafanyaVPN.Entities.Users.User;

namespace NafanyaVPN.Entities.Payments;

public class PaymentMessageService(
    ITelegramBotClient botClient, 
    IPaymentMessageRepository paymentMessageRepository) : IPaymentMessageService
{
    public async Task CreateAsync(Message message, User user)
    {
        var newPaymentMessageBuilder = new PaymentMessageBuilder()
            .WithTelegramChatId(message.Chat.Id)
            .WithUpdatedAt(DateTimeUtils.GetMoscowNowTime())
            .WithTelegramMessageId(message.MessageId)
            .WithUser(user);
        
        if (user.PaymentMessage is null)
        {
            var newPaymentMessage = newPaymentMessageBuilder
                .WithCreatedAt(DateTimeUtils.GetMoscowNowTime())
                .Build();
            await paymentMessageRepository.CreateAsync(newPaymentMessage);
        }
        else
        {
            await botClient.DeleteMessageAsync(user.PaymentMessage.TelegramChatId, user.PaymentMessage.TelegramMessageId);
            
            var newPaymentMessage = newPaymentMessageBuilder
                .WithCreatedAt(user.PaymentMessage.CreatedAt)
                .Build();
            await paymentMessageRepository.UpdateAsync(newPaymentMessage);
        }
    }

    public async Task ClearPaymentMessageAsync(long userId)
    {
        await paymentMessageRepository.DeleteByUserIdAsync(userId);
    }
}