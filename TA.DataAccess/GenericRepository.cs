using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TA.Entities.Interfaces;

namespace TA.DataAccess
{
    public abstract class GenericRepository : IRepository
    {
        protected DbContext Context { get; set; }

        private bool _disposed;

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class GenericRepository<T> : GenericRepository, IRepository<T> where T : class, IIdentifier<Guid>
    {
        protected DbSet<T> DbSet { get; set; }

        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual IQueryable<T> All(string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.AsQueryable();
            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));
            var result = query.AsQueryable();
            return result;
        }
        public virtual T Find(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.FirstOrDefault(predicate);
            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));
            var result = query.FirstOrDefault(predicate);
            return result;
        }        
        public virtual T FindByKey(Guid key)
        {
            return DbSet.Find(key);
        }      
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (predicate == null) return All(includes);
            if (includes == null || !includes.Any()) return DbSet.Where(predicate).AsQueryable();
            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));
            var result = query.Where(predicate).AsQueryable();
            return result;
        }
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null)
        {
            var skipCount = index * size;
            IQueryable<T> resetSet;
            if (includes != null && includes.Any())
            {
                var query = DbSet.Include(includes.First());
                query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));
                resetSet = predicate != null ? query.Where(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                resetSet = predicate != null ? DbSet.Where(predicate).AsQueryable() : DbSet.AsQueryable();
            }
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            var result = resetSet.AsQueryable();
            return result;
        }

        public EntityEntry<T> Create(T item)
        {
            var result = Context.Set<T>().Add(item);
            Context.SaveChanges();
            return result;
        }

        public T Update(T item)
        {
            T exist = Context.Set<T>().Find(item.Id);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(item);
                Context.SaveChanges();
            }
            return exist;
        }

        public void Delete(T item)
        {
            using (var context = Context)
            {
                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }
    }
}
