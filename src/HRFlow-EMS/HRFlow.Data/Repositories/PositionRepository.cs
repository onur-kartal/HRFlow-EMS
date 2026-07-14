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
    public class PositionRepository : GenericRepository<Position>,IPositionRepository
    {
        public PositionRepository(HRFlowDbContext context)
           : base(context)
        {
        }

        public async Task<List<Position>> GetPositionListAsync()
        {
            return await _context.Positions
               .Where(x => !x.IsDeleted)
               .ToListAsync();
        }
    }
}
