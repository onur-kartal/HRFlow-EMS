using HRFlow.Business.DTOs.Logging;

namespace HRFlow.Business.Interfaces
{
    public interface IRequestLogService
    {
        Task LogAsync(RequestLogCreateDto dto);
        Task<List<RequestLogListDto>> GetListAsync();
    }
}
