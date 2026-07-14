using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Interfaces
{
    public interface IDepartmentRepository :IGenericRepository<Department>
    {
        Task<List<Department>> GetDepartmentListAsync();
    }
}
