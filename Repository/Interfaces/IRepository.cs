using System.Linq.Expressions;

namespace Logex.API.Repository.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
