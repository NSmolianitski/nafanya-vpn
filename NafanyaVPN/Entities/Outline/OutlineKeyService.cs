namespace NafanyaVPN.Entities.Outline;

public class OutlineKeyService(IOutlineKeyRepository outlineKeysRepository) : IOutlineKeyService
{
    public async Task<OutlineKey> CreateAsync(OutlineKey model) =>
        await outlineKeysRepository.CreateAsync(model);

    public async Task EnableKeyAsync(int keyId)
    {
        var key = await outlineKeysRepository.GetByIdAsync(keyId);
        key.Enabled = true;
        
        await outlineKeysRepository.UpdateAsync(key);
    }

    public async Task DisableKeyAsync(int keyId)
    {
        var key = await outlineKeysRepository.GetByIdAsync(keyId);
        key.Enabled = false;
        
        await outlineKeysRepository.UpdateAsync(key);
    }
}