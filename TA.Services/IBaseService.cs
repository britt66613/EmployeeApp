using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TA.Entities.Interfaces;
using TA.Entities.Models;

namespace TA.Services
{
    public interface IBaseService<T> where T : class, IIdentifier<Guid>
    {
        IEnumerable<T> All(string[] includes = null);

        T Find(Expression<Func<T, bool>> predicate, string[] includes = null);

        T FindByKey(Guid id);

        IEnumerable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null);

        T Create(T entity);

        void Update(T entity);

        void Update(params T[] entities);

        void Delete(Guid id);

        void Delete(T entity);

    }
}
