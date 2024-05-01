namespace NafanyaVPN.Entities.Outline;

public class OutlineKeysService(IOutlineKeyRepository outlineKeysRepository) : IOutlineKeysService
{
    public async Task<OutlineKey> CreateAsync(OutlineKey model) =>
        await outlineKeysRepository.CreateAsync(model);
}