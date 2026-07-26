using HRFlow.Entities.Enums;

namespace HRFlow.Business.DTOs.Logging
{
    public class AuditLogCreateDto
    {
        public AuditModule Module { get; set; }
        public AuditAction Action { get; set; }
        public int? EntityId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
