using NafanyaVPN.Constants;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers;
using NafanyaVPN.Services.CommandHandlers.Commands.UserInput;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services;

public class TelegramStateService(IUserService userService, CheckCustomPaymentSumCommand checkCustomPaymentSumCommand)
    : ITelegramStateService
{
    private readonly Dictionary<string, ICommand<UserInputDto>> _commands = new()
    {
        { TelegramUserStateConstants.CustomPaymentSum, checkCustomPaymentSumCommand },
    };

    public async Task<bool> UserHasState(long telegramUserId)
    {
        var user = await userService.GetAsync(telegramUserId);
        return !string.IsNullOrWhiteSpace(user.TelegramState);
    }

    public async Task HandleStateAsync(Message message)
    {
        var user = await userService.GetAsync(message.From!.Id);
        
        if (!_commands.TryGetValue(user.TelegramState, out var command))
            throw new NoSuchCommandException(user.TelegramState);

        var userInputDto = new UserInputDto(message.Text!, message, message.From);
        await command.Execute(userInputDto);
    }
}