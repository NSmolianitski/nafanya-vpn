using NafanyaVPN.Database;

namespace NafanyaVPN.Entities.Withdraws;

public class WithdrawRepository(NafanyaVPNContext db) : IWithdrawRepository
{
    public Withdraw AddWithoutSaving(Withdraw model)
    {
        var withdraw = db.Withdraws.Add(model);
        return withdraw.Entity;
    }

    public void AddAllWithoutSaving(IEnumerable<Withdraw> models)
    {
        foreach (var model in models)
            AddWithoutSaving(model);
    }
}