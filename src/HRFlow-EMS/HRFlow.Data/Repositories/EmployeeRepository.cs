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

        public async Task<List<Employee>> GetEmployeeListAsync()
        {
            return await _context.Employees
        .Include(x => x.Department)
        .Include(x => x.Position)
        .ToListAsync();
        }
    }
}
