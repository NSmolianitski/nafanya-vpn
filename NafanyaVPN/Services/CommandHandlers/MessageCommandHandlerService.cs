using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers.Commands.Messages;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services.CommandHandlers;

public class MessageCommandHandlerService : ICommandHandlerService<Message>
{
    private readonly Dictionary<string, ICommand<Message>> _commands;

    public MessageCommandHandlerService(
        SendAccountDataCommand sendAccountDataCommand,
        SendPaymentSumChooseCommand sendPaymentSumChooseCommand,
        SendOutlineKeyCommand sendOutlineKeyCommand,
        SendInstructionCommand sendInstructionCommand,
        SendHelloCommand sendHelloCommand)
    {
        _commands = new Dictionary<string, ICommand<Message>>
        {
            { MainKeyboardConstants.Account, sendAccountDataCommand },
            { MainKeyboardConstants.Donate, sendPaymentSumChooseCommand },
            { MainKeyboardConstants.GetKey, sendOutlineKeyCommand },
            { MainKeyboardConstants.Instruction, sendInstructionCommand },
            { MainKeyboardConstants.Hello, sendHelloCommand },
        };
    }

    public async Task HandleCommand(Message data)
    {
        var text = data.Text!;
        if (!_commands.TryGetValue(text, out var command))
            command = _commands[MainKeyboardConstants.Hello];

        await command.Execute(data);
    }
}