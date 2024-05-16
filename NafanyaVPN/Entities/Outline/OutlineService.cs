using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Outline;

public class OutlineService(
    IConfiguration configuration,
    IOutlineKeyService outlineKeyService,
    ILogger<OutlineService> logger)
    : IOutlineService
{
    private readonly string _instructionText = Resources.Strings.OutlineInstruction;

    private readonly OutlineManager.Outline _outline = new(
        configuration
        [
            $"{OutlineConstants.SettingsSectionName}:" +
            $"{OutlineConstants.ApiUrl}"
        ]
    );

    public async Task CreateOutlineKeyForUser(User user)
    {
        var keyAccessUrl = CreateNewKeyInOutlineManager(user.TelegramUserName, user.TelegramUserId);

        user.OutlineKey = await outlineKeyService.CreateAsync(
            new OutlineKey
            {
                User = user, AccessUrl = keyAccessUrl
            });
    }

    private string CreateNewKeyInOutlineManager(string userName, long userId)
    {
        var key = _outline.CreateKey();

        if (!_outline.RenameKey(key.Id, $"{userName} ({userId})"))
            logger.LogError("Ключ Outline не был переименован. " +
                            "Username: {Username} UserId: {UserId}", userName, userId);

        return key.AccessUrl;
    }

    public string GetKeyByIdFromOutlineManager(int keyId)
    {
        var key = _outline.GetKeyById(keyId);
        return key.AccessUrl;
    }

    public async Task EnableKeyAsync(int keyId)
    {
        await outlineKeyService.EnableKeyAsync(keyId);

        if (!_outline.DeleteDataLimit(keyId))
            logger.LogError("Не удалось убрать лимит у ключа: {KeyId}", keyId);
    }

    public async Task DisableKeyAsync(int keyId)
    {
        await outlineKeyService.DisableKeyAsync(keyId);
        
        if (!_outline.AddDataLimit(keyId, 0))
            logger.LogError("Не удалось добавить лимит ключу: {KeyId}", keyId);
    }

    public void DeleteKeyFromOutlineManager(int keyId)
    {
        if (!_outline.DeleteKey(keyId))
            logger.LogError("Не удалось удалить ключ: {KeyId}", keyId);
    }

    public string GetInstruction() => _instructionText;
}