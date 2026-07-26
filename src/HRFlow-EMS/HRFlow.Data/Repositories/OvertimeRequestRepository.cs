using HRFlow.Common.Enums;
using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class OvertimeRequestRepository : GenericRepository<OvertimeRequest>, IOvertimeRequestRepository
    {
        public OvertimeRequestRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<List<OvertimeRequest>> GetOvertimeRequestListAsync()
        {
            return await OvertimeRequestQuery().ToListAsync();
        }

        public async Task<List<OvertimeRequest>> GetOvertimeRequestsByEmployeeIdAsync(int employeeId)
        {
            return await OvertimeRequestQuery()
                .Where(x => x.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<List<OvertimeRequest>> GetPendingOvertimeRequestsAsync()
        {
            return await OvertimeRequestQuery()
                .Where(x => x.Status == OvertimeStatus.Pending)
                .OrderBy(x => x.WorkDate)
                .ThenBy(x => x.StartTime)
                .ToListAsync();
        }

        public async Task<bool> HasTimeConflictAsync(
            int employeeId,
            DateTime workDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null)
        {
            return await _context.OvertimeRequests
                .Where(x => !x.IsDeleted &&
                            x.EmployeeId == employeeId &&
                            x.WorkDate.Date == workDate.Date &&
                            (x.Status == OvertimeStatus.Pending || x.Status == OvertimeStatus.Approved))
                .Where(x => !excludeId.HasValue || x.Id != excludeId.Value)
                .AnyAsync(x => startTime < x.EndTime && endTime > x.StartTime);
        }

        private IQueryable<OvertimeRequest> OvertimeRequestQuery()
        {
            return _context.OvertimeRequests
                .Where(x => !x.IsDeleted)
                .Include(x => x.Employee)
                .OrderByDescending(x => x.WorkDate)
                .ThenByDescending(x => x.StartTime);
        }
    }
}
