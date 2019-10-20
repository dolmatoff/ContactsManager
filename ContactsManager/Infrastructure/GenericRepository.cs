using ContactsManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactsManager.Infrastructure
{
    public class GenericRepository<T> : IAsyncRepository<T> where T : class
    {

        #region Fields

        protected RepositoryContext Context;

        #endregion

        public GenericRepository(RepositoryContext context)
        {
            Context = context;
        }

        #region Public Methods

        public ValueTask<T> GetById(int id) => Context.Set<T>().FindAsync(id);

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task Add(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public Task Update(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }

        public Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPaged(int page, int pageSize)
        {
            return await Context.Set<T>().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWherePaged(Expression<Func<T, bool>> predicate, int page, int pageSize)
        {
            return await Context.Set<T>().Where(predicate).Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<int> CountAll() => Context.Set<T>().CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().CountAsync(predicate);

        #endregion

    }
}
