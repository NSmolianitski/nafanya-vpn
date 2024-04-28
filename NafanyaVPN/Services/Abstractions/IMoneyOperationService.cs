using NafanyaVPN.Models;

namespace NafanyaVPN.Services.Abstractions;

public interface IMoneyOperationService
{
    Task<MoneyOperation> GetByLabelAsync(string label);
}