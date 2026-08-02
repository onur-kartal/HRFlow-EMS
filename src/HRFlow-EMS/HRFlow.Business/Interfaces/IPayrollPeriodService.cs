using HRFlow.Business.DTOs.Payroll;
using HRFlow.Entities.Enums;

namespace HRFlow.Business.Interfaces
{
    public interface IPayrollPeriodService
    {
        Task<List<PayrollPeriodListDto>> GetListAsync();
        Task<PayrollPeriodDetailDto?> GetDetailAsync(int id);
        Task CreateAsync(PayrollPeriodCreateDto dto);
        Task GeneratePayrollsAsync(int id);
        Task ApproveAsync(int id);
        Task RevertApprovalAsync(int id);
        Task MarkAsPaidAsync(int id);
        Task ChangeStatusAsync(int id, PayrollPeriodStatus status);
    }
}
