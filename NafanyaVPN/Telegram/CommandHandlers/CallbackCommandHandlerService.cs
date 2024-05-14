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
    BackToPaymentSumCommand backToPaymentSumCommand,
    ToggleRenewalCommand toggleRenewalCommand,
    ToggleRenewalNotificationsCommand toggleRenewalNotificationsCommand,
    ToggleSubEndNotificationsCommand toggleSubEndNotificationsCommand)
    : ICommandHandlerService<CallbackQueryDto>
{
    private readonly Dictionary<string, ICommand<CallbackQueryDto>> _commands = new()
    {
        { CallbackConstants.ConfirmPaymentSum, confirmPaymentSumCommand },
        { CallbackConstants.CustomPaymentSum, customPaymentSumCommand },
        { CallbackConstants.ConfirmCustomPaymentSum, confirmCustomPaymentSumCommand },
        { CallbackConstants.BackToPaymentSum, backToPaymentSumCommand },
        { CallbackConstants.ToggleRenewal, toggleRenewalCommand },
        { CallbackConstants.ToggleRenewalNotifications, toggleRenewalNotificationsCommand },
        { CallbackConstants.ToggleSubEndNotifications, toggleSubEndNotificationsCommand }
    };

    public async Task HandleCommand(CallbackQueryDto data)
    {
        var query = data.CallbackQuery;
        if (!_commands.TryGetValue(query, out var command))
            throw new NoSuchCommandException(query);

        await command.Execute(data);
    }
}