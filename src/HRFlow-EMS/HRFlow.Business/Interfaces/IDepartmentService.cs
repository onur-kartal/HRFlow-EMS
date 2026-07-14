using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface IDepartmentService : IGenericService<Department>
    {
        Task<List<DepartmentListDto>> GetDepartmentListAsync();

        Task CreateAsync(DepartmentCreateDto dto);

        Task<DepartmentUpdateDto?> GetByIdForUpdateAsync(int id);

        Task UpdateAsync(DepartmentUpdateDto dto);

    }
}
