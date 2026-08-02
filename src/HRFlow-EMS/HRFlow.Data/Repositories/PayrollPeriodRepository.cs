using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class PayrollPeriodRepository : GenericRepository<PayrollPeriod>, IPayrollPeriodRepository
    {
        public PayrollPeriodRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<bool> ExistsAsync(int year, int month)
        {
            return await _context.PayrollPeriods
                .AnyAsync(x => !x.IsDeleted && x.Year == year && x.Month == month);
        }

        public async Task<List<PayrollPeriod>> GetListAsync()
        {
            return await _context.PayrollPeriods
                .Where(x => !x.IsDeleted)
                .Include(x => x.EmployeePayrolls)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PayrollPeriod?> GetDetailAsync(int id)
        {
            return await _context.PayrollPeriods
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.EmployeePayrolls)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Department)
                .Include(x => x.EmployeePayrolls)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Position)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
