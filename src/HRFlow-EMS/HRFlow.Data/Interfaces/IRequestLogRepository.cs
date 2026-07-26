using HRFlow.Entities.Logging;

namespace HRFlow.Data.Interfaces
{
    public interface IRequestLogRepository
    {
        Task AddAsync(RequestLog requestLog);
        Task<List<RequestLog>> GetListAsync();
    }
}
