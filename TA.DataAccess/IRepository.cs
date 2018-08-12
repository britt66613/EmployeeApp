using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TA.Entities.Interfaces;

namespace TA.DataAccess
{
    public interface IRepository : IDisposable
    {
        void Save();

        Task<int> SaveAsync();
    }

    public interface IRepository<T> : IRepository where T : class, IIdentifier<Guid>
    {
        IQueryable<T> All(string[] includes = null);

        T Find(Expression<Func<T, bool>> predicate, string[] includes = null);

        T FindByKey(Guid key);

        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null);

        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null);

        EntityEntry<T> Create(T item);

        T Update(T item);

        void Delete(T item);
    }
}
