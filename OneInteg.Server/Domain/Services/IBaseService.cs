using System.Linq.Expressions;

namespace OneInteg.Server.Domain.Services
{
    public interface IBaseService<T> : IDisposable
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> Update(T entity);
        Task Delete(T entity);
    }
}
