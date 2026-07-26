using HRFlow.Common.Enums;

namespace HRFlow.Business.DTOs.OvertimeRequest
{
    public class OvertimeRequestListDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime WorkDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalHours { get; set; }

        public string Description { get; set; } = string.Empty;

        public OvertimeStatus Status { get; set; }
    }
}
