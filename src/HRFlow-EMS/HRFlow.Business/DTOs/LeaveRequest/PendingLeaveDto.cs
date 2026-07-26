using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveRequest
{
    public class PendingLeaveDto
    {
        public int Id { get; set; }

        public string EmployeeFullName { get; set; } = string.Empty;

        public string LeaveTypeName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }
    }
}
