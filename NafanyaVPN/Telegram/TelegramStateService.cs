using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.UserInput;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Telegram;

public class TelegramStateService(CheckCustomPaymentSumCommand checkCustomPaymentSumCommand)
    : ITelegramStateService
{
    private readonly Dictionary<string, ICommand<UserInputDto>> _commands = new()
    {
        { CallbackConstants.CustomPaymentSum, checkCustomPaymentSumCommand },
    };

    public bool UserHasState(User user)
    {
        return !string.IsNullOrWhiteSpace(user.TelegramState);
    }

    public bool CommandExists(string userTelegramState)
    {
        return _commands.ContainsKey(userTelegramState);
    }

    public async Task HandleStateAsync(MessageDto data)
    {
        if (!_commands.TryGetValue(data.User.TelegramState, out var command))
            throw new NoSuchCommandException(data.User.TelegramState);

        var message = data.Message;
        var userInputDto = new UserInputDto(message.Text!, message, message.From!);
        await command.Execute(userInputDto);
    }
}