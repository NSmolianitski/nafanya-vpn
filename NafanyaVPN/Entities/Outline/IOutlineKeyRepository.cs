namespace NafanyaVPN.Entities.Outline;

public interface IOutlineKeyRepository
{
    Task<OutlineKey> CreateAsync(OutlineKey model);
    IQueryable<OutlineKey> GetAll();
    Task<bool> DeleteAsync(OutlineKey model);
    Task<OutlineKey> UpdateAsync(OutlineKey model);
    Task UpdateAllAsync(IEnumerable<OutlineKey> models);
}