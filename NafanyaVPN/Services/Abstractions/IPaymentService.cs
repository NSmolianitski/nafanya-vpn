using yoomoney_api.quickpay;

namespace NafanyaVPN.Services.Abstractions;

public interface IPaymentService
{
    Quickpay GetPaymentForm(decimal sum, string paymentLabel);
    Task<string> ListenForPayment(string paymentLabel);
}