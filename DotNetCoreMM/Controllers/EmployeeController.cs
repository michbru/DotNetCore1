using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCoreMM.Services;
using DotNetCoreMM.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreMM.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        //    private readonly IDataRepository<Employee> _dataRepository;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employee
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Employee> employees = _employeeService.GetAll();
            return Ok(employees);
        }

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> Get(long id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {

                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Bad Request");
            }

           var ret = await _employeeService.AddAsync(employee);
            // return CreatedAtRoute("Get", new { Id = employee.EmployeeId }, employee);
            return Ok(ret);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null.");
            }

            //var employeeToUpdate = await _employeeService.GetByIdAsync(id);
            //if (employeeToUpdate == null)
            //{
            //    return NotFound("The Employee record couldn't be found.");
            //}

           var ret = await _employeeService.UpdateAsync(employee);
            return Ok(ret);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Employee employee = _employeeService.GetByIdAsync(id).Result;
            if (employee == null)
            {
                return NotFound("The Employee record couldn't be found.");
            }

           _employeeService.Delete(employee);
            return NoContent();
        }
    }
}
