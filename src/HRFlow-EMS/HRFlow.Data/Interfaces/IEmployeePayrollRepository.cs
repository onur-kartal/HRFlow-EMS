using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Data.Interfaces
{
    public interface IEmployeePayrollRepository : IGenericRepository<EmployeePayroll>
    {
        Task<bool> ExistsAsync(int payrollPeriodId, int employeeId);

        Task<List<EmployeePayroll>> GetByPeriodAsync(int payrollPeriodId);

        Task<List<EmployeePayroll>> GetManagementListAsync();

        Task<List<EmployeePayroll>> GetByEmployeeAsync(int employeeId);

        Task<EmployeePayroll?> GetDetailAsync(int id);

        Task<List<Employee>> GetActiveEmployeesAsync();

        Task<decimal> GetApprovedOvertimeHoursAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);
    }
}
