using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Database.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User model);
    IQueryable<User> GetAll();
    Task<bool> DeleteAsync(User model);
    Task<User> UpdateAsync(User model);
    Task UpdateAllAsync(IEnumerable<User> models);
}