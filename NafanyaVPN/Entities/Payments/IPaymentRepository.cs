namespace NafanyaVPN.Entities.Payments;

public interface IPaymentRepository
{
    Task<Payment> GetByLabelAsync(string label);
    Task<Payment?> TryGetByLabelAsync(string label);
    Task<Payment> CreateAsync(Payment model);
    Task<bool> DeleteAsync(Payment model);
    Task<Payment> UpdateAsync(Payment model);
    Task UpdateAllAsync(IEnumerable<Payment> models);
}