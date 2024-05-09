using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.CommandHandlers;

public class MessageCommandHandlerService(
    SendAccountDataCommand sendAccountDataCommand,
    SendPaymentSumChooseCommand sendPaymentSumChooseCommand,
    SendOutlineKeyCommand sendOutlineKeyCommand,
    SendInstructionCommand sendInstructionCommand,
    SendHelloCommand sendHelloCommand)
    : ICommandHandlerService<MessageDto>
{
    private readonly Dictionary<string, ICommand<MessageDto>> _commands = new()
    {
        { MainKeyboardConstants.Account, sendAccountDataCommand },
        { MainKeyboardConstants.Donate, sendPaymentSumChooseCommand },
        { MainKeyboardConstants.GetKey, sendOutlineKeyCommand },
        { MainKeyboardConstants.Instruction, sendInstructionCommand },
        { MainKeyboardConstants.Hello, sendHelloCommand },
    };

    public async Task HandleCommand(MessageDto data)
    {
        var text = data.Message.Text!;
        if (!_commands.TryGetValue(text, out var command))
            command = _commands[MainKeyboardConstants.Hello];

        await command.Execute(data);
    }
}