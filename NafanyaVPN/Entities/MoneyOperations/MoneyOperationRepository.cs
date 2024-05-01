using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.MoneyOperations;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Database.Repositories;

public class MoneyOperationRepository(NafanyaVPNContext db) : IMoneyOperationRepository
{
    private IQueryable<MoneyOperation> Items => db.MoneyOperations;

    public async Task<MoneyOperation> GetByLabelAsync(string label)
    {
        var moneyOperation = await TryGetByLabelAsync(label) ??
                             throw new NoSuchEntityException(
                                 $"Money operation with label: \"{label}\" does not exist. " +
                                 $"Repository: \"{GetType().Name}\".");

        return moneyOperation;
    }
    
    public async Task<MoneyOperation?> TryGetByLabelAsync(string label)
    {
        var moneyOperation = await Items.FirstOrDefaultAsync(o => o.Label == label);
        return moneyOperation;
    }
    
    public async Task<MoneyOperation> CreateAsync(MoneyOperation model)
    {
        var moneyOperation = await db.MoneyOperations.AddAsync(model);
        await db.SaveChangesAsync();
        return moneyOperation.Entity;
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