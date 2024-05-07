using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Payments;

public class PaymentMessageRepository(NafanyaVPNContext db) : IPaymentMessageRepository
{
    private IQueryable<PaymentMessage> Items => db.PaymentMessages;
    
    public async Task<PaymentMessage> GetByUserIdAsync(long userId)
    {
        var paymentMessage = await TryGetByUserIdAsync(userId) ??
                      throw new NoSuchEntityException(
                          $"Payment with label: \"{userId}\" does not exist. " +
                          $"Repository: \"{GetType().Name}\".");

        return paymentMessage;
    }
    
    public async Task<PaymentMessage?> TryGetByUserIdAsync(long userId)
    {
        return await Items.FirstOrDefaultAsync(p => p.UserId == userId);
    }
    
    public async Task CreateAsync(PaymentMessage paymentMessage)
    {
        await db.PaymentMessages.AddAsync(paymentMessage);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentMessage paymentMessage)
    {
        db.PaymentMessages.Update(paymentMessage);
        await db.SaveChangesAsync();
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var paymentMessage = await GetByUserIdAsync(userId);
        db.Remove(paymentMessage);
        await db.SaveChangesAsync();
    }
}