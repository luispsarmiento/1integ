using OneInteg.Server.Domain.Repositories;
using System.Linq.Expressions;

namespace OneInteg.Server.Domain.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> repository;
        public BaseService(IBaseRepository<T> repository)
        {
            this.repository = repository;
        }
        public Task<T> Add(T entity)
        {
            return repository.Add(entity);
        }

        public Task Delete(T entity)
        {
            return repository.Delete(entity);
        }

        public void Dispose()
        {
        }

        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return repository.Find(predicate);
        }

        public Task<T> Update(T entity)
        {
            return repository.Update(entity);
        }
    }
}
