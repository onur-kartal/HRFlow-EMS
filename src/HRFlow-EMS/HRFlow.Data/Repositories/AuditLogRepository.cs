using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly HRFlowDbContext _context;

        public AuditLogRepository(HRFlowDbContext context) => _context = context;

        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetListAsync()
        {
            return await _context.AuditLogs.AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
