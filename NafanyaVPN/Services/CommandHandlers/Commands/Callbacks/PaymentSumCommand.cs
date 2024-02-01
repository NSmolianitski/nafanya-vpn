using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers.Commands.Messages;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Callbacks;

public class PaymentSumCommand : ICommand<CallbackQueryDto>
{
    private readonly IReplyService _replyService;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger;

    public PaymentSumCommand(
        IReplyService replyService,
        IPaymentService paymentService,
        ILogger<SendPaymentSumChooseCommand> logger)
    {
        _replyService = replyService;
        _logger = logger;
        _paymentService = paymentService;
    }

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = decimal.Parse(data.Payload);
        await _paymentService.SendPaymentForm(paymentSum, data.User.Id);
        
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentSum}");
    }
}