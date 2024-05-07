namespace NafanyaVPN.Entities.Outline;

public interface IOutlineKeyService
{
    Task<OutlineKey> CreateAsync(OutlineKey model);
    Task EnableKeyAsync(int keyId);
    Task DisableKeyAsync(int keyId);
}