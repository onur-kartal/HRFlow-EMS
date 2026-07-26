using HRFlow.Business.DTOs.Logging;

namespace HRFlow.Business.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditLogCreateDto dto);
        Task<List<AuditLogListDto>> GetListAsync();
    }
}
