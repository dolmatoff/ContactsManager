using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactsManager.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {

        ValueTask<T> GetById(int id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetPaged(int page, int pageSize);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWherePaged(Expression<Func<T, bool>> predicate, int page, int pageSize);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

    }
}
