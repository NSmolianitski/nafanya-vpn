using System.Text;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.PaymentNotifications;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.DTOs;
using NafanyaVPN.Utils;
using Newtonsoft.Json;
using yoomoney_api.quickpay;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ConfirmPaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IUserService userService,
    IPaymentMessageService paymentMessageService,
    ILogger<PaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<PaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = StringUtils.ParseSum(data.Payload);
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await paymentMessageService.RemoveTelegramPaymentMessageAsync(user.Id);
        await replyService.EditMessageAsync(data.Message,
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");

        await SendMoney(quickpay);
    }
    
    // TODO: убрать (добавлено для тестов) //
    //////////////////////////////////////////
    private async Task SendMoney(Quickpay quickpay)
    {
        using (var client = new HttpClient())
        {
            var values = new Dictionary<string, string>
            {
                { "notification_type", "p2p-incoming" },
                { "bill_id", "" },
                { "amount", $"{quickpay.Sum}" },
                { "datetime", $"{DateTimeUtils.GetMoscowNowTime()}" },
                { "codepro", "false" },
                { "sender", "41001000040" },
                { "sha1_hash", "9860448f-7fb7-4d30-949f-241454851273" },
                { "test_notification", "false" },
                { "operation_label", "" },
                { "operation_id", "" },
                { "currency", "643" },
                { "label", $"{quickpay.Label}" }
            };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync($"http://localhost:5219/api/v1/payment-notification", content);

            // if (response.IsSuccessStatusCode)
            // {
            //     var responseContent = await response.Content.ReadAsStringAsync();
            // }
            // else
            // {
            // }
        }
    }
}