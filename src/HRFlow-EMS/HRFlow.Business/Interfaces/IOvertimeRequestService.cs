using HRFlow.Business.DTOs.OvertimeRequest;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Business.Interfaces
{
    public interface IOvertimeRequestService : IGenericService<OvertimeRequest>
    {
        Task CreateAsync(OvertimeRequestCreateDto dto);

        Task<List<OvertimeRequestListDto>> GetMyRequestsAsync();

        Task<List<OvertimeRequestListDto>> GetPendingRequestsAsync();

        Task<List<OvertimeRequestListDto>> GetAllRequestsAsync();

        Task ApproveAsync(int id);

        Task RejectAsync(int id);

        Task CancelAsync(int id);

        Task ChangeStatusAsync(OvertimeRequestStatusChangeDto dto);
    }
}
