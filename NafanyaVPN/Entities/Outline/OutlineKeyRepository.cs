using NafanyaVPN.Database;

namespace NafanyaVPN.Entities.Outline;

public class OutlineKeyRepository(NafanyaVPNContext db) : IOutlineKeyRepository
{
    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        var outlineKey = await db.OutlineKeys.AddAsync(model);
        await db.SaveChangesAsync();
        return outlineKey.Entity;
    }

    public IQueryable<OutlineKey> GetAll()
    {
        return db.OutlineKeys;
    }

    public async Task<bool> DeleteAsync(OutlineKey model)
    {
        db.OutlineKeys.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<OutlineKey> UpdateAsync(OutlineKey model)
    {
        db.OutlineKeys.Update(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<OutlineKey> models)
    {
        foreach (var model in models)
        {
            db.OutlineKeys.Update(model);
        }
        await db.SaveChangesAsync();
    }
}