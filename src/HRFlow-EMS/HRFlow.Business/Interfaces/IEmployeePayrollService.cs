using HRFlow.Business.DTOs.Payroll;

namespace HRFlow.Business.Interfaces
{
    public interface IEmployeePayrollService
    {
        Task<List<EmployeePayrollListDto>> GetManagementListAsync();

        Task<EmployeePayrollDetailDto?> GetDetailAsync(int id);

        Task UpdateAsync(EmployeePayrollUpdateDto dto);

        Task ApproveAsync(int id);

        Task MarkAsPaidAsync(int id);

        Task<List<MyPayrollListDto>> GetMyPayrollsAsync();

        Task<EmployeePayrollDetailDto?> GetMyDetailAsync(int id);
    }
}
