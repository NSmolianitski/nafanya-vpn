using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;

namespace NafanyaVPN.Database.Repositories;

public class OutlineKeyRepository : IBaseRepository<OutlineKey>
{
    private readonly NafanyaVPNContext _db;

    public OutlineKeyRepository(NafanyaVPNContext db)
    {
        _db = db;
    }
    
    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        var outlineKey = await _db.OutlineKeys.AddAsync(model);
        await _db.SaveChangesAsync();
        return outlineKey.Entity;
    }

    public IQueryable<OutlineKey> GetAll()
    {
        return _db.OutlineKeys;
    }

    public async Task<bool> DeleteAsync(OutlineKey model)
    {
        _db.OutlineKeys.Remove(model);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<OutlineKey> UpdateAsync(OutlineKey model)
    {
        _db.OutlineKeys.Update(model);
        await _db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<OutlineKey> models)
    {
        foreach (var model in models)
        {
            _db.OutlineKeys.Update(model);
        }
        await _db.SaveChangesAsync();
    }
}