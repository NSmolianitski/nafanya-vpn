namespace NafanyaVPN.Entities.Outline;

public interface IOutlineService
{
    string CreateNewKeyInOutlineManager(string userName, long userId);
    string GetKeyByIdFromOutlineManager(int keyId);
    Task EnableKeyAsync(int keyId);
    Task DisableKeyAsync(int keyId);
    void DeleteKeyFromOutlineManager(int keyId);
    string GetInstruction();
}