using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Payments;

public class PaymentRepository(NafanyaVPNContext db) : IPaymentRepository
{
    private IQueryable<Payment> Items => db.Payments;

    public async Task<Payment> GetByLabelAsync(string label)
    {
        var payment = await TryGetByLabelAsync(label) ??
                             throw new NoSuchEntityException(
                                 $"Payment with label: \"{label}\" does not exist. " +
                                 $"Repository: \"{GetType().Name}\".");

        return payment;
    }
    
    public async Task<Payment?> TryGetByLabelAsync(string label)
    {
        var payment = await Items.FirstOrDefaultAsync(p => p.Label == label);
        return payment;
    }
    
    public async Task<Payment> CreateAsync(Payment model)
    {
        var payment = await db.Payments.AddAsync(model);
        await db.SaveChangesAsync();
        return payment.Entity;
    }

    public async Task<bool> DeleteAsync(Payment model)
    {
        db.Payments.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<Payment> UpdateAsync(Payment model)
    {
        db.Payments.Update(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<Payment> models)
    {
        foreach (var model in models)
        {
            db.Payments.Update(model);
        }
        await db.SaveChangesAsync();
    }
}