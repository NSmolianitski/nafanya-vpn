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
        var realOutlineKey = CreateNewKeyInOutlineManager(user.TelegramUserName, user.TelegramUserId);

        user.OutlineKey = await outlineKeyService.CreateAsync(
            new OutlineKey
            {
                OutlineId = realOutlineKey.Id,
                User = user, 
                AccessUrl = realOutlineKey.AccessUrl
            });
    }

    private OutlineManager.Types.OutlineKey CreateNewKeyInOutlineManager(string userName, long userId)
    {
        var key = _outline.CreateKey();

        if (!_outline.RenameKey(key.Id, $"{userName} ({userId})"))
        {
            logger.LogError("Ключ Outline не был переименован. " +
                            "Username: {Username} UserId: {UserId}", userName, userId);
        }

        return key;
    }

    public string GetKeyByIdFromOutlineManager(int keyId)
    {
        var realOutlineKey = _outline.GetKeyById(keyId);
        return realOutlineKey.AccessUrl;
    }

    public async Task EnableKeyAsync(OutlineKey key)
    {
        await outlineKeyService.EnableKeyAsync(key.Id);

        if (!_outline.DeleteDataLimit(key.OutlineId))
        {
            logger.LogError("Не удалось убрать лимит у ключа " +
                            "с ID (из БД, не из OutlineManager!): {KeyId}", key.Id);
        }
    }

    public async Task DisableKeyAsync(OutlineKey key)
    {
        await outlineKeyService.DisableKeyAsync(key.Id);

        if (!_outline.AddDataLimit(key.OutlineId, 0))
        {
            logger.LogError("Не удалось добавить лимит ключу " +
                            "c ID (из БД, не из OutlineManager!): {KeyId}", key.Id);
        }
    }

    public void DeleteKeyFromOutlineManager(OutlineKey key)
    {
        if (!_outline.DeleteKey(key.OutlineId))
        {
            logger.LogError("Не удалось удалить ключ " +
                            "c ID (из БД, не из OutlineManager!): {KeyId}", key.Id);
        }
    }

    public string GetInstruction() => _instructionText;
}