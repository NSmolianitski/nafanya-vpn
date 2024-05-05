using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers;

public class CallbackCommandHandlerService(
    PaymentSumCommand paymentSumCommand,
    CustomPaymentSumCommand customPaymentSumCommand)
    : ICommandHandlerService<CallbackQueryDto>
{
    private readonly Dictionary<string, ICommand<CallbackQueryDto>> _commands = new()
    {
        { CallbackConstants.PaymentSum, paymentSumCommand },
        { CallbackConstants.CustomPaymentSum, customPaymentSumCommand },
    };

    public async Task HandleCommand(CallbackQueryDto data)
    {
        var query = data.CallbackQuery;
        if (!_commands.TryGetValue(query, out var command))
            throw new NoSuchCommandException(query);

        await command.Execute(data);
    }
}