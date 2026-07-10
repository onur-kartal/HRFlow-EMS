using HRFlow.Business.DTOs.Employee;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Task<List<EmployeeListDto>> GetEmployeeListAsync();
        Task CreateAsync(EmployeeCreateDto dto);

        Task<List<Department>> GetDepartmentsAsync();

        Task<List<Position>> GetPositionsAsync();

        Task<EmployeeUpdateDto?> GetByIdForUpdateAsync(int id);

        Task UpdateAsync(EmployeeUpdateDto dto);


    }
}
