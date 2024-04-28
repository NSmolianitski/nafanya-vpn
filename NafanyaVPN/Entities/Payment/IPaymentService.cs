using yoomoney_api.quickpay;

namespace NafanyaVPN.Entities.Payment;

public interface IPaymentService
{
    Quickpay GetPaymentForm(decimal sum, string paymentLabel);
    Task<string> ListenForPayment(string paymentLabel);
}