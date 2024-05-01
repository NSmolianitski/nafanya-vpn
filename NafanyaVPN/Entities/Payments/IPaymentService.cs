using yoomoney_api.quickpay;

namespace NafanyaVPN.Entities.Payments;

public interface IPaymentService
{
    Task<Payment> GetByLabelAsync(string label);
    Quickpay GetPaymentForm(decimal sum, string paymentLabel);
}