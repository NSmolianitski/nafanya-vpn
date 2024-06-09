using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Users;

public class UserRepository(NafanyaVPNContext db) : IUserRepository
{
    public async Task<User> CreateAsync(User model)
    {
        var user = await db.Users.AddAsync(model);
        await db.SaveChangesAsync();
        return user.Entity;
    }

    public async Task<List<User>> GetAllWithForeignKeysAsync()
    {
        return await db.Users
            .Include(u => u.OutlineKey)
            .Include(u => u.Subscription).ThenInclude(s => s.SubscriptionPlan)
            .ToListAsync();
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await TryGetByIdAsync(id) ??
                   throw new NoSuchEntityException(
                       $"User with id: \"{id}\" does not exist. " +
                       $"Repository: \"{GetType().Name}\".");

        return user;
    }
    
    public async Task<User?> TryGetByIdAsync(int id)
    {
        var user = await db.Users
            .Include(u => u.OutlineKey)
            .Include(u => u.Subscription).ThenInclude(s => s.SubscriptionPlan)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }
    
    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        var user = await TryGetByTelegramIdAsync(telegramId) ??
                      throw new NoSuchEntityException(
                          $"User with telegram id: \"{telegramId}\" does not exist. " +
                          $"Repository: \"{GetType().Name}\".");

        return user;
    }
    
    public async Task<User?> TryGetByTelegramIdAsync(long telegramId)
    {
        var user = await db.Users
            .Include(u => u.OutlineKey)
            .Include(u => u.Subscription).ThenInclude(s => s.SubscriptionPlan)
            .Include(u => u.PaymentMessage)
            .FirstOrDefaultAsync(u => u.TelegramUserId == telegramId);
        return user;
    }

    public async Task<bool> DeleteAsync(User model)
    {
        db.Users.Remove(model);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<User> UpdateAsync(User model)
    {
        UpdateWithoutSaving(model);
        await db.SaveChangesAsync();
        return model;
    }

    public async Task UpdateAllAsync(IEnumerable<User> models)
    {
        foreach (var model in models)
        {
            UpdateWithoutSaving(model);
        }
        await db.SaveChangesAsync();
    }

    private EntityEntry<User> UpdateWithoutSaving(User model)
    {
        model.UpdatedAt = DateTimeUtils.GetMoscowNowTime();
        return db.Users.Update(model);
    }
}