using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Payments;

public class PaymentRepository(NafanyaVPNContext db) : IPaymentRepository
{
    private IQueryable<Payment> Items => db.Payments;

    public async Task<Payment> GetByLabelAsync(string label)
    {
        return await TryGetByLabelAsync(label) ?? 
               throw new NoSuchEntityException(
                   $"Payment with label: \"{label}\" does not exist. " + 
                   $"Repository: \"{GetType().Name}\".");
    }
    
    public async Task<Payment?> TryGetByLabelAsync(string label)
    {
        return await Items
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Label == label);
    }
    
    public async Task<Payment> CreateAsync(Payment model)
    {
        var payment = db.Payments.Add(model);
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
        UpdateWithoutSaving(model);
        await db.SaveChangesAsync();
        return model;
    }

    public async Task UpdateAllAsync(IEnumerable<Payment> models)
    {
        foreach (var model in models)
        {
            UpdateWithoutSaving(model);
        }
        await db.SaveChangesAsync();
    }

    private EntityEntry<Payment> UpdateWithoutSaving(Payment model)
    {
        model.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
        return db.Payments.Update(model);
    }
}