using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Exceptions;

namespace NafanyaVPN.Entities.Users;

public class UserRepository(NafanyaVPNContext db) : IUserRepository
{
    public async Task<User> CreateAsync(User model)
    {
        var user = await db.Users.AddAsync(model);
        await db.SaveChangesAsync();
        return user.Entity;
    }

    public async Task<List<User>> GetAllWithOutlineKeysAsync()
    {
        return await db.Users.Include(u => u.OutlineKey).ToListAsync();
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
        var user = await db.Users.Include(u => u.Subscription)
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
        db.Users.Update(model);
        await db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<User> models)
    {
        foreach (var model in models)
        {
            db.Users.Update(model);
        }
        await db.SaveChangesAsync();
    }
}