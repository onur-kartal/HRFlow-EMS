using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveType
{
    public class LeaveTypeUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DefaultDays { get; set; }
        public bool IsPaid { get; set; }
    }
}
