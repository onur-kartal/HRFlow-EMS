using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        public DepartmentService(IGenericRepository<Department> repository)
        : base(repository)
        {
        }
    }
}
