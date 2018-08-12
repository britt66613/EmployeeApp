using System;
using System.Collections.Generic;
using TA.Entities.Entity;
using TA.Entities.Interfaces;
using TA.Entities.Models;

namespace TA.Services.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        new IEnumerable<Employee> Filter(FilterModel model, string[] includes = null);
    }
}
