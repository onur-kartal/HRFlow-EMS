using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Interfaces
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<int> GetLeaveRequestCountAsync();

        Task<List<LeaveRequest>> GetLeaveRequestListAsync();

        Task<List<LeaveRequest>> GetLastLeaveRequestsAsync(int count);

        Task<bool> HasDateConflictAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeId = null);
    }
}
