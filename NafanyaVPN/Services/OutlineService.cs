using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;
using OutlineManager;

namespace NafanyaVPN.Services;

public class OutlineService(IConfiguration configuration, ILogger<OutlineService> logger)
    : IOutlineService
{
    private readonly Outline _outline = new(
        configuration
        [
            $"{OutlineConstants.SettingsSectionName}:" +
            $"{OutlineConstants.ApiUrl}"
        ]
    );

    public string GetNewKey(string userName, long userId)
    {
        var key = _outline.CreateKey();
        
        if (!_outline.RenameKey(key.Id, $"{userName} ({userId})"))
            logger.LogError("Ключ Outline не был переименован.");

        return key.AccessUrl;
    }
    
    public string GetKeyById(int keyId)
    {
        var key = _outline.GetKeyById(keyId);
        return key.AccessUrl;
    }

    public void EnableKey(int keyId)
    {
        if (!_outline.DeleteDataLimit(keyId))
            logger.LogError($"Не удалось убрать лимит у ключа: {keyId}");
    }

    public void DisableKey(int keyId)
    {
        if (!_outline.AddDataLimit(keyId, 0))
            logger.LogError($"Не удалось добавить лимит ключу: {keyId}");
    }
    
    public void DeleteKey(int keyId)
    {
        if (!_outline.DeleteKey(keyId))
            logger.LogError($"Не удалось удалить ключ: {keyId}");
    }
}