using NafanyaVPN.Entities.MoneyOperations;

namespace NafanyaVPN.Database.Repositories;

public interface IMoneyOperationRepository
{
    Task<MoneyOperation> GetByLabelAsync(string label);
    Task<MoneyOperation?> TryGetByLabelAsync(string label);
    Task<MoneyOperation> CreateAsync(MoneyOperation model);
    Task<bool> DeleteAsync(MoneyOperation model);
    Task<MoneyOperation> UpdateAsync(MoneyOperation model);
    Task UpdateAllAsync(IEnumerable<MoneyOperation> models);
}