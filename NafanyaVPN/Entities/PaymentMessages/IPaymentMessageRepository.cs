namespace NafanyaVPN.Entities.PaymentMessages;

public interface IPaymentMessageRepository
{
    Task<PaymentMessage> GetByUserIdAsync(long userId);
    Task<PaymentMessage?> TryGetByUserIdAsync(long userId);
    Task CreateAsync(PaymentMessage paymentMessage);
    Task UpdateAsync(PaymentMessage paymentMessage);
    Task DeleteByUserIdAsync(long userId);
}