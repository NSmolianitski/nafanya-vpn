using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Telegram.Abstractions;

public interface ITelegramStateService
{
    public bool UserHasState(User user);
    public bool CommandExists(string userTelegramState);
    public Task HandleStateAsync(MessageDto message);
}