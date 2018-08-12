using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TA.Entities.Entity;
using TA.Entities.Models;
using TA.Services.Interfaces;

namespace TA.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        [Route("GetByKey")]
        [HttpPost]
        public ActionResult GetByKey([FromBody] GetModel item)
        {
            var result = Guid.Parse(item.Id);
            return Ok(employeeService.FindByKey(result));
        }

        //http://localhost:50884/api/employee/GetAll?includes=Location&includes=Action
        [Route("GetAll")]
        [HttpPost]
        public ActionResult GetAll()
        {
            var result = employeeService.All().ToList();
            return Ok(result);
        }

        [Route("Filter")]
        [HttpPost]
        public ActionResult Filter([FromBody] FilterModel model)
        {
            var result = employeeService.Filter(model);
            return Ok(result);
        }

        // POST api/values
        [Route("AddEmployee")]
        [HttpPost]
        public ActionResult Post([FromBody]Employee employee)
        {
            //restaurantRepo.Create(employee);
            if (ModelState.IsValid)
            {
                employeeService.Create(employee);

                return Ok();
            }

            else return BadRequest(ModelState);
            
        }

        // PUT api/values/5
        [Route("UpdateEmployee")]
        [HttpPut]
        public ActionResult Put([FromBody]Employee restaurant)
        {
            if (ModelState.IsValid)
            {
                employeeService.Update(restaurant);
                return Ok();
            }
            else return BadRequest();
            
        }

        // DELETE api/values/5
        [Route("Delete")]
        [HttpPost]
        public ActionResult Delete([FromBody] Employee employee)
        {            
            employeeService.Delete(employee);
            return Ok();
        }
    }
}
