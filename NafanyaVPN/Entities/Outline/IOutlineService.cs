using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Outline;

public interface IOutlineService
{
    Task CreateOutlineKeyForUser(User user);
    string GetKeyByIdFromOutlineManager(int keyId);
    Task EnableKeyAsync(int keyId);
    Task DisableKeyAsync(int keyId);
    void DeleteKeyFromOutlineManager(int keyId);
    string GetInstruction();
}