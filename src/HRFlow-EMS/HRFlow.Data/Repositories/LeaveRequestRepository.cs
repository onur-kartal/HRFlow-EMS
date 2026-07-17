using HRFlow.Common.Enums;
using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HRFlowDbContext context)
            : base(context)
        {
        }
        public async Task<List<LeaveRequest>> GetLastLeaveRequestsAsync(int count)
        {
            return await LeaveRequestQuery()
                .OrderByDescending(x => x.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetLeaveRequestCountAsync()
        {
            return await _context.LeaveRequests
                .Where(x => !x.IsDeleted)
                .CountAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestListAsync()
        {
            return await LeaveRequestQuery()
                .ToListAsync();
        }

        public async Task<bool> HasDateConflictAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeId = null)
        {
            return await _context.LeaveRequests
                .Where(x => !x.IsDeleted && x.Status != LeaveStatus.Rejected)
                .Where(x => !excludeId.HasValue || x.Id != excludeId.Value)
                .AnyAsync(x =>
                x.EmployeeId == employeeId &&
                startDate <= x.EndDate &&
                endDate >= x.StartDate);
        }

        private IQueryable<LeaveRequest> LeaveRequestQuery()
        {
            return _context.LeaveRequests
               .Where(x => !x.IsDeleted)
               .Include(x => x.Employee)
               .Include(x => x.LeaveType);
        }
    }
}
