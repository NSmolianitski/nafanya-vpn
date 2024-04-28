namespace NafanyaVPN.Entities.MoneyOperations;

public interface IMoneyOperationService
{
    Task<MoneyOperation> GetByLabelAsync(string label);
}