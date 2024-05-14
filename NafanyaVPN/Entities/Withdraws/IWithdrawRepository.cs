namespace NafanyaVPN.Entities.Withdraws;

public interface IWithdrawRepository
{
    Withdraw AddWithoutSaving(Withdraw model);
    void AddAllWithoutSaving(IEnumerable<Withdraw> models);
    // Task<Withdraw> UpdateAsync(Withdraw model);
    // Task UpdateAllAsync(IEnumerable<Withdraw> models);
}