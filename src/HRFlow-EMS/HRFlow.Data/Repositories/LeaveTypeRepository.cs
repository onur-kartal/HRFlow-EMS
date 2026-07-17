using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<int> GetLeaveTypeCountAsync()
        {
            return await _context.LeaveTypes
                .Where(x => !x.IsDeleted)
                .CountAsync();
        }

        public async Task<List<LeaveType>> GetLeaveTypeListAsync()
        {
            return await _context.LeaveTypes
               .Where(x => !x.IsDeleted)
               .ToListAsync();
        }
    }
}
