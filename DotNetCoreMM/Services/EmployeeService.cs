using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreMM.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreMM.Services
{
    public class EmployeeService : IEmployeeService
    {
        readonly MMDatabaseContext _employeeContext;

        public EmployeeService(MMDatabaseContext context)
        {
            _employeeContext = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeContext.Employees.ToList();
        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            try
            {
                var ret = await _employeeContext.Employees.FirstOrDefaultAsync(e => e.id == id);
                return ret;
            }
            catch(Exception ex)
            {
                return null; 
            }
         
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            _employeeContext.Employees.Add(entity);
            _employeeContext.SaveChanges();
            return entity;
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
 
            _employeeContext.Employees.Update(entity);
            _employeeContext.SaveChanges();
            return entity;
        }

        public void Delete(Employee employee)
        {
            _employeeContext.Employees.Remove(employee);
            _employeeContext.SaveChanges();
        }
    }
}
