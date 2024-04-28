using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class MoneyOperationService(IBaseRepository<MoneyOperation> moneyOperationRepository)
    : IMoneyOperationService
{
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
        var user = await moneyOperationRepository.GetAll().FirstOrDefaultAsync(o => o.Label == label);
        return user;
    }
}