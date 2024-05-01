using yoomoney_api.account;
using yoomoney_api.notification;
using yoomoney_api.quickpay;

namespace NafanyaVPN.Entities.Payments;

public class YoomoneyPaymentService : IPaymentService
{
    private readonly Client _client;
    private readonly Account _accountInfo;
    private readonly string _wallet;
    private readonly string _secret;
    private readonly string _serverAddress;
    private readonly int _serverPort;
    
    private readonly IPaymentRepository _paymentRepository;
    
    public YoomoneyPaymentService(IConfiguration configuration, IPaymentRepository paymentRepository)
    {
        var yoomoneyConfig = configuration.GetRequiredSection(YoomoneyConstants.Yoomoney);
        var clientId = yoomoneyConfig[$"{YoomoneyConstants.ClientId}"]!;
        var redirectUri = yoomoneyConfig[$"{YoomoneyConstants.RedirectUri}"]!;
        var accessToken = yoomoneyConfig[$"{YoomoneyConstants.AccessToken}"]!;
        
        _wallet = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Wallet}"]!;
        _secret = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Secret}"]!;

        _serverAddress = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.NafanyaIp}"]!;
        _serverPort = 5219;
        
        _paymentRepository = paymentRepository;

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
        return new Quickpay(_wallet, "shop", sum, paymentLabel, PaymentMethodConstants.BankCard);
    }

    public async Task<string> ListenForPayment(string paymentLabel)
    {
        var paymentListenerToYooMoney = new PaymentListenerToYooMoney(paymentLabel, DateTime.Today, _secret);
        var paymentResult = await paymentListenerToYooMoney.Listen(_serverAddress, _serverPort);
        return paymentResult;
    }
    
    public async Task<Payment> GetByLabelAsync(string label) =>
        await _paymentRepository.GetByLabelAsync(label);
}