using HRFlow.Common.Enums;
using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class EmployeePayrollRepository : GenericRepository<EmployeePayroll>, IEmployeePayrollRepository
    {
        public EmployeePayrollRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<List<EmployeePayroll>> GetManagementListAsync()
        {
            return await EmployeePayrollQuery()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int payrollPeriodId, int employeeId)
        {
            return await _context.EmployeePayrolls
                .AnyAsync(x => !x.IsDeleted &&
                               x.PayrollPeriodId == payrollPeriodId &&
                               x.EmployeeId == employeeId);
        }

        public async Task<List<EmployeePayroll>> GetByPeriodAsync(int payrollPeriodId)
        {
            return await EmployeePayrollQuery()
                .Where(x => x.PayrollPeriodId == payrollPeriodId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<EmployeePayroll>> GetByEmployeeAsync(int employeeId)
        {
            return await EmployeePayrollQuery()
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.PayrollPeriod.Year)
                .ThenByDescending(x => x.PayrollPeriod.Month)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EmployeePayroll?> GetDetailAsync(int id)
        {
            return await EmployeePayrollQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Employee>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(x => !x.IsDeleted && x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<decimal> GetApprovedOvertimeHoursAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.OvertimeRequests
                .Where(x => !x.IsDeleted &&
                            x.EmployeeId == employeeId &&
                            x.Status == OvertimeStatus.Approved &&
                            x.WorkDate >= startDate.Date &&
                            x.WorkDate <= endDate.Date)
                .SumAsync(x => (decimal?)x.TotalHours) ?? 0m;
        }

        private IQueryable<EmployeePayroll> EmployeePayrollQuery()
        {
            return _context.EmployeePayrolls
                .Where(x => !x.IsDeleted)
                .Include(x => x.PayrollPeriod)
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Department)
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Position);
        }
    }
}
