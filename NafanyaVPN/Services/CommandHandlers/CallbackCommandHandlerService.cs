using NafanyaVPN.Constants;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers.Commands.Callbacks;

namespace NafanyaVPN.Services.CommandHandlers;

public class CallbackCommandHandlerService : ICommandHandlerService<CallbackQueryDto>
{
    private readonly Dictionary<string, ICommand<CallbackQueryDto>> _commands;

    public CallbackCommandHandlerService(
        PaymentSumCommand paymentSumCommand,
        CustomPaymentSumCommand customPaymentSumCommand)
    {
        _commands = new Dictionary<string, ICommand<CallbackQueryDto>>
        {
            { CallbackConstants.PaymentSum, paymentSumCommand },
            { CallbackConstants.CustomPaymentSum, customPaymentSumCommand },
        };
    }

    public async Task HandleCommand(CallbackQueryDto data)
    {
        var query = data.CallbackQuery;
        if (!_commands.TryGetValue(query, out var command))
            throw new NoSuchCommandException(query);

        await command.Execute(data);
    }
}