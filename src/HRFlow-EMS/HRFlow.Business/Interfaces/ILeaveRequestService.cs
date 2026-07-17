using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.Position;
using HRFlow.Entities.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface ILeaveRequestService : IGenericService<LeaveRequest>
    {
        Task<List<LeaveRequestListDto>> GetLeaveRequestListAsync();

        Task<int> GetLeaveRequestCountAsync();

        Task CreateAsync(LeaveRequestCreateDto dto);

        Task<LeaveRequestUpdateDto?> GetByIdForUpdateAsync(int id);

        Task UpdateAsync(LeaveRequestUpdateDto dto);

        Task ApproveAsync(int id);

        Task RejectAsync(int id);
    }
}
