using NafanyaVPN.Exceptions;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Callbacks;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.CommandHandlers;

public class CallbackCommandHandlerService(
    ConfirmPaymentSumCommand confirmPaymentSumCommand,
    CustomPaymentSumCommand customPaymentSumCommand,
    ConfirmCustomPaymentSumCommand confirmCustomPaymentSumCommand,
    BackToPaymentSumCommand backToPaymentSumCommand)
    : ICommandHandlerService<CallbackQueryDto>
{
    private readonly Dictionary<string, ICommand<CallbackQueryDto>> _commands = new()
    {
        { CallbackConstants.ConfirmPaymentSum, confirmPaymentSumCommand },
        { CallbackConstants.CustomPaymentSum, customPaymentSumCommand },
        { CallbackConstants.ConfirmCustomPaymentSum, confirmCustomPaymentSumCommand },
        { CallbackConstants.BackToPaymentSum, backToPaymentSumCommand },
    };

    public async Task HandleCommand(CallbackQueryDto data)
    {
        var query = data.CallbackQuery;
        if (!_commands.TryGetValue(query, out var command))
            throw new NoSuchCommandException(query);

        await command.Execute(data);
    }
}