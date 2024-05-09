using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Abstractions;

public interface ITelegramStateService
{
    public bool UserHasState(User user);
    public bool CommandExists(string userTelegramState);
    public Task HandleStateAsync(MessageDto message);
}