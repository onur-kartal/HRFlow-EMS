using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.LeaveType;
using HRFlow.Entities.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface ILeaveTypeService : IGenericService<LeaveType>
    {
        Task<List<LeaveTypeListDto>> GetleaveTypeListAsync();
        Task CreateAsync(LeaveTypeCreateDto dto);
        Task<LeaveTypeUpdateDto?> GetByIdForUpdateAsync(int id);
        Task UpdateAsync(LeaveTypeUpdateDto dto);
        Task<int> GetLeaveTypeCountAsync();
        Task<List<LeaveTypeLookupDto>> GetLeaveTypeLookupAsync();
    }
}
