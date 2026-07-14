using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(HRFlowDbContext context)
           : base(context)
        {
        }

        public async Task<List<Department>> GetDepartmentListAsync()
        {
            return await _context.Departments
               .Where(x => !x.IsDeleted)
               .ToListAsync();
        }
    }
}
