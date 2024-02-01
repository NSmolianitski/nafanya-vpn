namespace NafanyaVPN.Services.Abstractions;

public interface IOutlineService
{
    string GetNewKey(string userName, long userId);
    string GetKeyById(int keyId);
    void EnableKey(int keyId);
    void DisableKey(int keyId);
    void DeleteKey(int keyId);
}