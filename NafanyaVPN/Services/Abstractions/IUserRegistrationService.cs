namespace NafanyaVPN.Services.Abstractions;

public interface IUserRegistrationService
{
    Task<bool> IsRegistered(long telegramUserId);
    Task RegisterUser(long telegramUserId, string telegramUserName);
}