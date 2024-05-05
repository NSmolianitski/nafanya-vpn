using NafanyaVPN.Entities.Users;
using yoomoney_api.quickpay;

namespace NafanyaVPN.Entities.Payments;

public interface IPaymentService
{
    Task<Payment> GetByLabelAsync(string label);
    Task<Payment> FinishPaymentAsync(Payment payment);
    Task<Quickpay> CreatePaymentFormAsync(decimal sum, User user);
}