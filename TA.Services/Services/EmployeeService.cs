using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TA.DataAccess.Db;
using TA.Entities.Entity;
using TA.Entities.Models;
using TA.Services.Interfaces;

namespace TA.Services.Services
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        public EmployeeService(EmployeeContext context) : base(context) { }

        public IEnumerable<Employee> Filter(FilterModel model, string[] includes = null)
        {
            Expression<Func<Employee, bool>> FilterExpression(FilterModel _model)
            {
                Expression<Func<Employee, bool>> expression = null;

                if (_model == null) return null;

                if (!String.IsNullOrEmpty(_model.Name))
                {
                    expression = x => x.Name.Contains(_model.Name.ToString());
                }

                if (!String.IsNullOrEmpty(_model.Position))
                {
                    expression = ExpressionCombiner.And<Employee>(expression, x => x.Position.Contains(_model.Position.ToString()));                    
                }

                return expression;
            }

            var result = base.Filter(FilterExpression(model), includes);
            return result;
        }

        public Employee Find(Expression<Func<Employee, bool>> predicate, string[] includes = null)
        {
            var result = base.Find((predicate), includes);
            return result;
        }

        public IEnumerable<Employee> GetAll(string[] includes = null)
        {
            var result = base.All(includes);
            return result;
        }

        public Employee FindByKey(Guid id)
        {
            var result = base.FindByKey(id);
            return result;
        }

        public Employee Create(Employee entity)
        {
            if (entity.StartTime == new DateTime()) entity.StartTime = DateTime.Now;
            var result = base.Create(entity);
            return result;
        }

        public void Update(Employee entity)
        {
            base.Update(entity);
        }

        public void Update(params Employee[] entities)
        {
            foreach (var entity in entities)
            {
                base.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            var result = base.FindByKey(id);

            base.Delete(result);
        }

        public void Delete(Employee entity)
        {
            base.Delete(entity);
        }
    }
}
