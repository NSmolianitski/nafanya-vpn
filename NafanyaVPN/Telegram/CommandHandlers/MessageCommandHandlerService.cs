using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Callbacks;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.CommandHandlers;

public class MessageCommandHandlerService(
    AccountDataCommand accountDataCommand,
    PaymentSumChooseCommand paymentSumChooseCommand,
    OutlineKeyCommand outlineKeyCommand,
    InstructionCommand instructionCommand,
    SettingsCommand settingsCommand,
    RenewSubscriptionCommand renewSubscriptionCommand,
    ToggleRenewalCommand toggleRenewalCommand,
    ToggleRenewalNotificationsCommand toggleRenewalNotificationsCommand,
    ToggleSubEndNotificationsCommand toggleSubEndNotificationsCommand,
    BackToMainMenuCommand backToMainMenuCommand,
    HelloCommand helloCommand)
    : ICommandHandlerService<MessageDto>
{
    private readonly Dictionary<string, ICommand<MessageDto>> _commands = new()
    {
        // Основное меню
        { MainKeyboardConstants.Account, accountDataCommand },
        { MainKeyboardConstants.Donate, paymentSumChooseCommand },
        { MainKeyboardConstants.GetKey, outlineKeyCommand },
        { MainKeyboardConstants.Instruction, instructionCommand },
        { MainKeyboardConstants.Settings, settingsCommand },
        { MainKeyboardConstants.RenewSubscription, renewSubscriptionCommand },
        { MainKeyboardConstants.Hello, helloCommand },
        
        // Меню настроек
        { MainKeyboardConstants.EnableRenewal, toggleRenewalCommand },
        { MainKeyboardConstants.DisableRenewal, toggleRenewalCommand },
        { MainKeyboardConstants.EnableRenewalNotifications, toggleRenewalNotificationsCommand },
        { MainKeyboardConstants.DisableRenewalNotifications, toggleRenewalNotificationsCommand },
        { MainKeyboardConstants.EnableSubEndNotifications, toggleSubEndNotificationsCommand },
        { MainKeyboardConstants.DisableSubEndNotifications, toggleSubEndNotificationsCommand },
        { MainKeyboardConstants.BackToMainMenu, backToMainMenuCommand },
    };

    public async Task HandleCommand(MessageDto data)
    {
        var text = data.Message.Text!;
        if (!_commands.TryGetValue(text, out var command))
            command = _commands[MainKeyboardConstants.Hello];

        await command.Execute(data);
    }
}