namespace NafanyaVPN.Database.Abstract;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T model);
    IQueryable<T> GetAll();
    Task<bool> DeleteAsync(T model);
    Task<T> UpdateAsync(T model);
    Task UpdateAllAsync(IEnumerable<T> models);
}