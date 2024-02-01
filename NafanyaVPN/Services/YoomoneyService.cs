using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;
using yoomoney_api.account;
using yoomoney_api.notification;
using yoomoney_api.quickpay;

namespace NafanyaVPN.Services;

public class YoomoneyService : IPaymentService
{
    private readonly Client _client;
    private readonly Account _accountInfo;
    private readonly string _wallet;
    private readonly string _secret;
    
    public YoomoneyService(IConfiguration configuration)
    {
        var yoomoneyConfig = configuration.GetRequiredSection(YoomoneyConstants.Yoomoney);
        var clientId = yoomoneyConfig[$"{YoomoneyConstants.ClientId}"]!;
        var redirectUri = yoomoneyConfig[$"{YoomoneyConstants.RedirectUri}"]!;
        var accessToken = yoomoneyConfig[$"{YoomoneyConstants.AccessToken}"]!;
        
        _wallet = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Wallet}"]!;
        _secret = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Secret}"]!;
        
        // _authorize = new Authorize(clientId: clientId, redirectUri: redirectUri, 
        //     scope: new [] 
        //     {
        //         "account-info",
        //         "operation-history",
        //         "operation-details",
        //         "incoming-transfers",
        //         "payment-p2p",
        //     });
        
        // _client = new Client(accessToken);
        // _accountInfo = _client.GetAccountInfo(accessToken);
        // _accountInfo.Print();
    }

    public async Task SendPaymentForm(decimal sum, long userId)
    {
        var label = GetUniqueLabel();
        var quickpay = GetQuickpayForm(sum, label);
        Console.WriteLine(quickpay.LinkPayment);
        
        var paymentListenerToYooMoney = new PaymentListenerToYooMoney(label, DateTime.Today, _secret);
        var resultPayment = await paymentListenerToYooMoney.Listen("http://127.0.0.1:4040",5000);
        Console.WriteLine(resultPayment);
    }

    private Quickpay GetQuickpayForm(decimal sum, string label)
    {
        return new Quickpay(_wallet, "shop", sum, label, PaymentType.BankCard);
    }
    
    private string GetUniqueLabel()
    {
        return Guid.NewGuid().ToString();
    }
}