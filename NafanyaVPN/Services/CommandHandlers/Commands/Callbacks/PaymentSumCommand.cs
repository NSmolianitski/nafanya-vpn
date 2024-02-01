using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers.Commands.Messages;
using NafanyaVPN.Utils;

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
        var paymentLabel = StringUtils.GetUniqueLabel();
        var quickpay = _paymentService.GetPaymentForm(paymentSum, paymentLabel);
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
        
        var paymentResult = await _paymentService.ListenForPayment(paymentLabel);
        
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentResult}");
    }
}