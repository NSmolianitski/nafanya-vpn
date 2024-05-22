using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Outline;

public interface IOutlineService
{
    Task CreateOutlineKeyForUser(User user);
    string GetKeyByIdFromOutlineManager(int keyId);
    Task EnableKeyAsync(OutlineKey key);
    Task DisableKeyAsync(OutlineKey key);
    void DeleteKeyFromOutlineManager(OutlineKey key);
    string GetInstruction();
}