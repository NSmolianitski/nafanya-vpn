namespace NafanyaVPN.Entities.Outline;

public interface IOutlineKeyRepository
{
    Task<OutlineKey> CreateAsync(OutlineKey model);
    IQueryable<OutlineKey> GetAll();
    Task<OutlineKey> GetByIdAsync(long keyId);
    Task<OutlineKey?> TryGetByIdAsync(long keyId);
    Task<bool> DeleteAsync(OutlineKey model);
    Task<OutlineKey> UpdateAsync(OutlineKey model);
    Task UpdateAllAsync(IEnumerable<OutlineKey> models);
}