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

        public async Task<List<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId)
        {
            return await LeaveRequestQuery()
                .Where(x => x.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<int> GetLeaveRequestCountByStatusAsync(LeaveStatus status, int? employeeId = null)
        {
            var query = _context.LeaveRequests
                .Where(x => !x.IsDeleted && x.Status == status);

            if (employeeId.HasValue)
            {
                query = query.Where(x => x.EmployeeId == employeeId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<LeaveRequest>> GetPendingLeaveRequestListAsync()
        {
            return await LeaveRequestQuery()
                .Where(x => x.Status == LeaveStatus.Pending)
                .OrderBy(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetPendingLeaveRequestsAsync(int count)
        {
            return await LeaveRequestQuery()
            .Where(x =>
                !x.IsDeleted &&
                 x.Status == LeaveStatus.Pending)
            .OrderBy(x => x.StartDate)
            .Take(count)
            .ToListAsync();
        }

        public async Task<int> GetTodayOnLeaveCountAsync()
        {
            var today = DateTime.Today;

            return await _context.LeaveRequests
                .Where(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveStatus.Approved &&
                    x.StartDate.Date <= today &&
                    x.EndDate.Date >= today)
                .Select(x => x.EmployeeId)
                .Distinct()
                .CountAsync();
        }

        public async Task<List<LeaveRequest>> GetUpcomingLeaveRequestsAsync(int count)
        {
            var today = DateTime.Today;
            var nextWeek = today.AddDays(7);

            return await LeaveRequestQuery()
                .Where(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveStatus.Approved &&
                    x.StartDate.Date > today &&
                    x.StartDate.Date <= nextWeek)
                .OrderBy(x => x.StartDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> HasDateConflictAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeId = null)
        {
            return await _context.LeaveRequests
                .Where(x => !x.IsDeleted &&
                            x.Status != LeaveStatus.Rejected &&
                            x.Status != LeaveStatus.Cancelled)
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
