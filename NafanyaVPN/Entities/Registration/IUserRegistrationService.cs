namespace NafanyaVPN.Entities.Registration;

public interface IUserRegistrationService
{
    Task<bool> IsRegistered(long telegramUserId);
    Task RegisterUser(long telegramUserId, string telegramUserName);
}