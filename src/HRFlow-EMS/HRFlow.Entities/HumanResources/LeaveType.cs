using HRFlow.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Entities.HumanResources
{
    public class LeaveType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int DefaultDays { get; set; }

        public bool IsPaid { get; set; }
    }
}
