using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<Employee>> GetEmployeeListAsync();
        Task<int> GetEmployeeCountAsync();
        Task<List<Employee>> GetLastEmployeesAsync(int count);
        Task<List<Employee>> GetEmployeesWithDepartmentAsync();
    }
}
