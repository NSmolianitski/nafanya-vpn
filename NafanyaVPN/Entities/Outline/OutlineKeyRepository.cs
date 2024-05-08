using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Outline;

public class OutlineKeyRepository(NafanyaVPNContext db) : IOutlineKeyRepository
{
    private IQueryable<OutlineKey> Items => db.OutlineKeys;
    
    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        var outlineKey = db.OutlineKeys.Add(model);
        await db.SaveChangesAsync();
        return outlineKey.Entity;
    }

    public IQueryable<OutlineKey> GetAll()
    {
        return db.OutlineKeys;
    }

    public async Task<OutlineKey> GetByIdAsync(long keyId)
    {
        var paymentMessage = await TryGetByIdAsync(keyId) ??
                             throw new NoSuchEntityException(
                                 $"Payment with label: \"{keyId}\" does not exist. " +
                                 $"Repository: \"{GetType().Name}\".");

        return paymentMessage;
    }
    
    public async Task<OutlineKey?> TryGetByIdAsync(long keyId)
    {
        return await Items.FirstOrDefaultAsync(p => p.Id == keyId);
    }
    
    public async Task<bool> DeleteAsync(OutlineKey model)
    {
        db.OutlineKeys.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<OutlineKey> UpdateAsync(OutlineKey model)
    {
        UpdateWithoutSaving(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<OutlineKey> models)
    {
        foreach (var model in models)
        {
            UpdateWithoutSaving(model);
        }
        await db.SaveChangesAsync();
    }
    
    private EntityEntry<OutlineKey> UpdateWithoutSaving(OutlineKey model)
    {
        model.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
        return db.OutlineKeys.Update(model);
    }
}