using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;
using OutlineManager;

namespace NafanyaVPN.Services;

public class OutlineService : IOutlineService
{
    private readonly Outline _outline;
    private readonly ILogger<OutlineService> _logger;

    public OutlineService(IConfiguration configuration, ILogger<OutlineService> logger)
    {
        _logger = logger;

        _outline = new Outline
        (
            configuration
            [
                $"{OutlineConstants.SettingsSectionName}:" +
                $"{OutlineConstants.ApiUrl}"
            ]
        );
    }

    public string GetNewKey(string userName, long userId)
    {
        var key = _outline.CreateKey();
        
        if (!_outline.RenameKey(key.Id, $"{userName} ({userId})"))
            _logger.LogError("Ключ Outline не был переименован.");

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
            _logger.LogError($"Не удалось убрать лимит у ключа: {keyId}");
    }

    public void DisableKey(int keyId)
    {
        if (!_outline.AddDataLimit(keyId, 0))
            _logger.LogError($"Не удалось добавить лимит ключу: {keyId}");
    }
    
    public void DeleteKey(int keyId)
    {
        if (!_outline.DeleteKey(keyId))
            _logger.LogError($"Не удалось удалить ключ: {keyId}");
    }
}