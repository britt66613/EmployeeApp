using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TA.DataAccess.Db;
using TA.DataAccess.Interface;
using TA.Entities.Entity;

namespace TA.DataAccess.ConcreteRepository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeContext context) : base(context) { }
    }
}
