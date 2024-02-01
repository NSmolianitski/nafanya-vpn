using Telegram.Bot.Types;

namespace NafanyaVPN.Services.Abstractions;

public interface ITelegramStateService
{
    public Task<bool> UserHasState(long telegramUserId);
    public Task HandleStateAsync(Message message);
}