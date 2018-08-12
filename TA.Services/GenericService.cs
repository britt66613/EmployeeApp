using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TA.DataAccess;
using TA.DataAccess.Db;
using TA.Entities.Interfaces;

namespace TA.Services
{
    public abstract class GenericService : IDisposable
    {
        private bool _disposed;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
        }
    }

    public abstract class GenericService<T> : GenericService, IBaseService<T> where T : class, IIdentifier<Guid>
    {
        protected IRepository<T> EntityRepo;

        public GenericService(EmployeeContext context)
        {
            EntityRepo = new GenericRepository<T>(context);
        }

        public virtual IEnumerable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            try
            {
                var result = EntityRepo.Filter(predicate, includes);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual IEnumerable<T> All(string[] includes = null)
        {
            try
            {
                var result = EntityRepo.All(includes);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual T Find(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            try
            {
                var result = EntityRepo.Find(predicate, includes);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual T FindByKey(Guid id)
        {
            try
            {
                var result = EntityRepo.FindByKey(id);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public virtual T Create(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentException(nameof(entity));
                var queryResult = EntityRepo.Create(entity);
                return queryResult.Entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                EntityRepo.Update(entity);
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void Update(params T[] entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    EntityRepo.Update(entity);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                EntityRepo.Delete(entity);
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                var queryResult = EntityRepo.FindByKey(id);

                EntityRepo.Delete(queryResult);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
