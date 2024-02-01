using NafanyaVPN.Constants;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers;
using NafanyaVPN.Services.CommandHandlers.Commands.UserInput;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services;

public class TelegramStateService : ITelegramStateService
{
    private readonly IUserService _userService;
    private readonly Dictionary<string, ICommand<UserInputDto>> _commands;

    public TelegramStateService(IUserService userService, CheckCustomPaymentSumCommand checkCustomPaymentSumCommand)
    {
        _userService = userService;
        _commands = new Dictionary<string, ICommand<UserInputDto>>
        {
            { TelegramUserStateConstants.CustomPaymentSum, checkCustomPaymentSumCommand },
        };
    }

    public async Task<bool> UserHasState(long telegramUserId)
    {
        var user = await _userService.GetAsync(telegramUserId);
        return !string.IsNullOrWhiteSpace(user.TelegramState);
    }

    public async Task HandleStateAsync(Message message)
    {
        var user = await _userService.GetAsync(message.From!.Id);
        
        if (!_commands.TryGetValue(user.TelegramState, out var command))
            throw new NoSuchCommandException(user.TelegramState);

        var userInputDto = new UserInputDto(message.Text!, message, message.From);
        await command.Execute(userInputDto);
    }
}