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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<int> GetEmployeeCountAsync()
        {
            return await _context.Employees
                .Where(x => !x.IsDeleted)
                .CountAsync();
        }

        public async Task<List<Employee>> GetEmployeeListAsync()
        {
            return await _context.Employees
                .Where(x => !x.IsDeleted)
                .Include(x => x.Department)
                .Include(x => x.Position)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetLastEmployeesAsync(int count)
        {
            return await EmployeeDetailQuery()
                .OrderByDescending(x => x.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
        private IQueryable<Employee> EmployeeQuery()
        {
            return _context.Employees
                .Where(x => !x.IsDeleted);
        }

        private IQueryable<Employee> EmployeeDetailQuery()
        {
            return EmployeeQuery()
                .Include(x => x.Department)
                .Include(x => x.Position);
        }

        public async Task<List<Employee>> GetEmployeesWithDepartmentAsync()
        {
            return await _context.Employees
                .Where(x => !x.IsDeleted)
                .Include(x => x.Department)
                .ToListAsync();
        }
    }
}
