using HRFlow.Data.Context;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Repositories
{
    public class PositionRepository : GenericRepository<Position>
    {
        public PositionRepository(HRFlowDbContext context)
           : base(context)
        {
        }
    }
}
