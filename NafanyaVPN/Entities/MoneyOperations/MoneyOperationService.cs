using NafanyaVPN.Database.Repositories;

namespace NafanyaVPN.Entities.MoneyOperations;

public class MoneyOperationService(IMoneyOperationRepository moneyOperationRepository)
    : IMoneyOperationService
{
    public async Task<MoneyOperation> GetByLabelAsync(string label) =>
        await moneyOperationRepository.GetByLabelAsync(label);
}