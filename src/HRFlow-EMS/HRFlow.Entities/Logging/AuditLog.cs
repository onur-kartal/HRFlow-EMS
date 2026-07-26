using HRFlow.Entities.Enums;

namespace HRFlow.Entities.Logging
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public AuditModule Module { get; set; }
        public AuditAction Action { get; set; }
        public int? EntityId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
    }
}
