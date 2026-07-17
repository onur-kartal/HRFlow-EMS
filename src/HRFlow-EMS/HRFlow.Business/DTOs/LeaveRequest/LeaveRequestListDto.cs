using HRFlow.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveRequest
{
    public class LeaveRequestListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string LeaveTypeName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public LeaveStatus Status { get; set; }

        public int TotalDays { get; set; }
    }
}
