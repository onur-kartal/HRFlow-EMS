using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Data.Interfaces
{
    public interface IOvertimeRequestRepository : IGenericRepository<OvertimeRequest>
    {
        Task<List<OvertimeRequest>> GetOvertimeRequestsByEmployeeIdAsync(int employeeId);

        Task<List<OvertimeRequest>> GetPendingOvertimeRequestsAsync();

        Task<List<OvertimeRequest>> GetOvertimeRequestListAsync();

        Task<bool> HasTimeConflictAsync(
            int employeeId,
            DateTime workDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null);
    }
}
