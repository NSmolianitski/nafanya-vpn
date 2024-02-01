using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;

namespace NafanyaVPN.Database.Repositories;

public class UserRepository : IBaseRepository<User>
{
    private readonly NafanyaVPNContext _db;

    public UserRepository(NafanyaVPNContext db)
    {
        _db = db;
    }

    public async Task<User> CreateAsync(User model)
    {
        var user = await _db.Users.AddAsync(model);
        await _db.SaveChangesAsync();
        return user.Entity;
    }

    public IQueryable<User> GetAll()
    {
        return _db.Users;
    }

    public async Task<bool> DeleteAsync(User model)
    {
        _db.Users.Remove(model);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<User> UpdateAsync(User model)
    {
        _db.Users.Update(model);
        await _db.SaveChangesAsync();
        return model;
    }
    
    public async Task UpdateAllAsync(IEnumerable<User> models)
    {
        foreach (var model in models)
        {
            _db.Users.Update(model);
        }
        await _db.SaveChangesAsync();
    }
}