using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;
using yoomoney_api.notification;
using yoomoney_api.quickpay;

namespace NafanyaVPN.Entities.Payments;

public class YoomoneyPaymentService(
    IConfiguration configuration,
    IUserRepository userRepository,
    IPaymentRepository paymentRepository) 
    : IPaymentService
{
    private readonly string _wallet = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Wallet}"]!;
    private readonly string _secret = configuration[$"{YoomoneyConstants.Yoomoney}:{YoomoneyConstants.Secret}"]!;
    
    private readonly string _serverAddress = configuration[$"{YoomoneyConstants.Yoomoney}:" +
                                                           $"{YoomoneyConstants.NafanyaIp}"]!;
    
    private readonly int _serverPort = int.Parse(configuration[$"{YoomoneyConstants.Yoomoney}:" +
                                                               $"{YoomoneyConstants.NafanyaPort}"]!);

    public async Task<Quickpay> CreatePaymentFormAsync(decimal sum, User user)
    {
        var paymentLabel = StringUtils.GetUniqueLabel();
        await CreatePaymentAsync(sum, user, paymentLabel);
        
        // TODO: убрать (добавлено для тестов) //
        //////////////////////////////////////////
        user.MoneyInRoubles += sum;
        await userRepository.UpdateAsync(user);
        //////////////////////////////////////////
        
        return new Quickpay(_wallet, "shop", sum, paymentLabel, 
            PaymentMethodConstants.BankCard);
    }

    private async Task<Payment> CreatePaymentAsync(decimal sum, User user, string paymentLabel)
    {
        var payment = new PaymentBuilder()
            .WithCreatedAt(DateTimeUtils.GetMoscowNowTime())
            .WithUpdatedAt(DateTimeUtils.GetMoscowNowTime())
            .WithUser(user)
            .WithSum(sum)
            .WithLabel(paymentLabel)
            .WithStatus(PaymentStatusType.Waiting)
            .Build();
        return await paymentRepository.CreateAsync(payment);
    }

    public async Task<Payment> GetByLabelAsync(string label) =>
        await paymentRepository.GetByLabelAsync(label);

    public async Task<Payment> FinishPaymentAsync(Payment payment)
    {
        payment.Status = PaymentStatusType.Finished;
        return await paymentRepository.UpdateAsync(payment);
    }

    // СОХРАНЕНО НА ВСЯКИЙ СЛУЧАЙ
    #region saved

    private void GetInitialYoomoneyData()
    {
        var yoomoneyConfig = configuration.GetRequiredSection(YoomoneyConstants.Yoomoney);
        var clientId = yoomoneyConfig[$"{YoomoneyConstants.ClientId}"]!;
        var redirectUri = yoomoneyConfig[$"{YoomoneyConstants.RedirectUri}"]!;
        var accessToken = yoomoneyConfig[$"{YoomoneyConstants.AccessToken}"]!;
        
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
    
    public async Task<string> ListenForPayment(string paymentLabel)
    {
        var paymentListenerToYooMoney = new PaymentListenerToYooMoney(paymentLabel, DateTime.Today, _secret);
        var paymentResult = await paymentListenerToYooMoney.Listen(_serverAddress, _serverPort);
        return paymentResult;
    }

    #endregion
}