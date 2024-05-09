using Telegram.Bot.Types;

namespace NafanyaVPN.Telegram.DTOs;

public record CallbackQueryDto(string CallbackQuery, string Payload, Message Message, User User)
{
    public string CallbackQuery { get; } = CallbackQuery;
    public string Payload { get; } = Payload;
    public Message Message { get; } = Message;
    public User User { get; } = User;
}