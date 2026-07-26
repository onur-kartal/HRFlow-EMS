using HRFlow.Entities.Logging;

namespace HRFlow.Data.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog auditLog);
        Task<List<AuditLog>> GetListAsync();
    }
}
