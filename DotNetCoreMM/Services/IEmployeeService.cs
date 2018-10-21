
using DotNetCoreMM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreMM.Services
{
    public interface IEmployeeService
    {
        Task<Employee> AddAsync(Employee entity);
        void Delete(Employee employee);
        Task<Employee> GetByIdAsync(long id);
        IEnumerable<Employee> GetAll();
        Task<Employee> UpdateAsync(Employee entity);
    }
}