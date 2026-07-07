using HRFlow.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Entities.Organization
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
