using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.Constants;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers;

public class MessageCommandHandlerService(
    SendAccountDataCommand sendAccountDataCommand,
    SendPaymentSumChooseCommand sendPaymentSumChooseCommand,
    SendOutlineKeyCommand sendOutlineKeyCommand,
    SendInstructionCommand sendInstructionCommand,
    SendHelloCommand sendHelloCommand)
    : ICommandHandlerService<Message>
{
    private readonly Dictionary<string, ICommand<Message>> _commands = new()
    {
        { MainKeyboardConstants.Account, sendAccountDataCommand },
        { MainKeyboardConstants.Donate, sendPaymentSumChooseCommand },
        { MainKeyboardConstants.GetKey, sendOutlineKeyCommand },
        { MainKeyboardConstants.Instruction, sendInstructionCommand },
        { MainKeyboardConstants.Hello, sendHelloCommand },
    };

    public async Task HandleCommand(Message data)
    {
        var text = data.Text!;
        if (!_commands.TryGetValue(text, out var command))
            command = _commands[MainKeyboardConstants.Hello];

        await command.Execute(data);
    }
}