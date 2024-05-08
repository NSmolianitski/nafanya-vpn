using Telegram.Bot.Types;
using User = NafanyaVPN.Entities.Users.User;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;

public record MessageDto(Message Message, User User)
{
    public Message Message { get; } = Message;
    public User User { get; } = User;
}