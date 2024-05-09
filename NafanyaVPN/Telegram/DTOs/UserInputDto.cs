using Telegram.Bot.Types;

namespace NafanyaVPN.Telegram.DTOs;

public record UserInputDto(string Payload, Message Message, User User)
{
    public string Payload { get; } = Payload;
    public Message Message { get; } = Message;
    public User User { get; } = User;
}