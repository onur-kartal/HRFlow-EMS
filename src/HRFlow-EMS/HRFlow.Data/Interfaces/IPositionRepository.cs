using HRFlow.Common.Interfaces;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Interfaces
{
    public interface IPositionRepository: IGenericRepository<Position>
    {
        Task<List<Position>> GetPositionListAsync();
        Task<int> GetPositionCountAsync();
    }
}
