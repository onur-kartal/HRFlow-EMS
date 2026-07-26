using HRFlow.Common.Enums;
using HRFlow.Entities.Base;

namespace HRFlow.Entities.HumanResources
{
    public class OvertimeRequest : BaseEntity
    {
        public int EmployeeId { get; set; }

        public DateTime WorkDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalHours { get; set; }

        public string Description { get; set; } = string.Empty;

        public OvertimeStatus Status { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
