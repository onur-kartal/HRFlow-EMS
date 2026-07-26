using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly HRFlowDbContext _context;

        public RequestLogRepository(HRFlowDbContext context) => _context = context;

        public async Task AddAsync(RequestLog requestLog)
        {
            await _context.RequestLogs.AddAsync(requestLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RequestLog>> GetListAsync()
        {
            return await _context.RequestLogs.AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
