using NafanyaVPN.Database;

namespace NafanyaVPN.Entities.Users;

public class UserRepository(NafanyaVPNContext db) : IUserRepository
{
    public async Task<User> CreateAsync(User model)
    {
        var user = await db.Users.AddAsync(model);
        await db.SaveChangesAsync();
        return user.Entity;
    }

    public IQueryable<User> GetAll()
    {
        return db.Users;
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