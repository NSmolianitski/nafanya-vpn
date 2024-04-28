using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Entities.MoneyOperations;

namespace NafanyaVPN.Database.Repositories;

public class MoneyOperationRepository(NafanyaVPNContext db) : IBaseRepository<MoneyOperation>
{
    public async Task<MoneyOperation> CreateAsync(MoneyOperation model)
    {
        var moneyOperation = await db.MoneyOperations.AddAsync(model);
        await db.SaveChangesAsync();
        return moneyOperation.Entity;
    }

    public IQueryable<MoneyOperation> GetAll()
    {
        return db.MoneyOperations;
    }

    public async Task<bool> DeleteAsync(MoneyOperation model)
    {
        db.MoneyOperations.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<MoneyOperation> UpdateAsync(MoneyOperation model)
    {
        db.MoneyOperations.Update(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<MoneyOperation> models)
    {
        foreach (var model in models)
        {
            db.MoneyOperations.Update(model);
        }
        await db.SaveChangesAsync();
    }
}