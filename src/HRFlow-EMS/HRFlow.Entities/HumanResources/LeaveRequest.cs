using HRFlow.Common.Enums;
using HRFlow.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Entities.HumanResources
{
    public class LeaveRequest : BaseEntity
    {
        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Description { get; set; }

        public LeaveStatus Status { get; set; }

        public Employee Employee { get; set; } = null!;

        public LeaveType LeaveType { get; set; } = null!;
    }
}
