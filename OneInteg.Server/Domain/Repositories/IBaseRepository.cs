using System.Linq.Expressions;

namespace OneInteg.Server.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> Update(T entity);
        Task Delete(T entity);
    }
}
