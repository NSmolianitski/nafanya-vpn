﻿using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.PaymentMessages;

public class PaymentMessageRepository(NafanyaVPNContext db) : IPaymentMessageRepository
{
    private IQueryable<PaymentMessage> Items => db.PaymentMessages;
    
    public async Task<PaymentMessage> GetByUserIdAsync(long userId)
    {
        var paymentMessage = await TryGetByUserIdAsync(userId) ??
                      throw new NoSuchEntityException(
                          $"Payment message with userId: \"{userId}\" does not exist. " +
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
        paymentMessage.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
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