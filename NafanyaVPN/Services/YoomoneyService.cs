using System.Net;
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
    private readonly string _serverAddress;
    private readonly int _serverPort;
    
    public YoomoneyService(IConfiguration configuration)
    {
        var yoomoneyConfig = configuration.GetRequiredSection(YoomoneyConstants.Yoomoney);
        var clientId = yoomoneyConfig[$"{YoomoneyConstants.ClientId}"]!;
        var redirectUri = yoomoneyConfig[$"{YoomoneyConstants.RedirectUri}"]!;
        var accessToken = yoomoneyConfig[$"{YoomoneyConstants.AccessToken}"]!;
        
        _wallet = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Wallet}"]!;
        _secret = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Secret}"]!;

        _serverAddress = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.NafanyaIp}"]!;
        _serverPort = 5000;

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

    public Quickpay GetPaymentForm(decimal sum, string paymentLabel)
    {
        return new Quickpay(_wallet, "shop", sum, paymentLabel, PaymentType.BankCard);
    }

    public async Task<string> ListenForPayment(string paymentLabel)
    {
        var paymentListenerToYooMoney = new PaymentListenerToYooMoney(paymentLabel, DateTime.Today, _secret);
        var paymentResult = await paymentListenerToYooMoney.Listen(_serverAddress, _serverPort);
        return paymentResult;
    }
}