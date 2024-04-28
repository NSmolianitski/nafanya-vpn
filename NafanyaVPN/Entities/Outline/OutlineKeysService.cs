using NafanyaVPN.Database.Abstract;

namespace NafanyaVPN.Entities.Outline;

public class OutlineKeysService(IBaseRepository<OutlineKey> outlineKeysRepository) : IOutlineKeysService
{
    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        return await outlineKeysRepository.CreateAsync(model);
    }
}