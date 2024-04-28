using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram;

public interface ITelegramStateService
{
    public Task<bool> UserHasState(long telegramUserId);
    public Task HandleStateAsync(Message message);
}