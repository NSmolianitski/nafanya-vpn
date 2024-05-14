using NafanyaVPN.Telegram.Abstractions;
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
    HelloCommand helloCommand)
    : ICommandHandlerService<MessageDto>
{
    private readonly Dictionary<string, ICommand<MessageDto>> _commands = new()
    {
        { MainKeyboardConstants.Account, accountDataCommand },
        { MainKeyboardConstants.Donate, paymentSumChooseCommand },
        { MainKeyboardConstants.GetKey, outlineKeyCommand },
        { MainKeyboardConstants.Instruction, instructionCommand },
        { MainKeyboardConstants.Settings, settingsCommand },
        { MainKeyboardConstants.Hello, helloCommand },
    };

    public async Task HandleCommand(MessageDto data)
    {
        var text = data.Message.Text!;
        if (!_commands.TryGetValue(text, out var command))
            command = _commands[MainKeyboardConstants.Hello];

        await command.Execute(data);
    }
}