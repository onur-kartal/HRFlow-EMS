using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Data.Interfaces
{
    public interface IPayrollPeriodRepository : IGenericRepository<PayrollPeriod>
    {
        Task<bool> ExistsAsync(int year, int month);

        Task<List<PayrollPeriod>> GetListAsync();

        Task<PayrollPeriod?> GetDetailAsync(int id);
    }
}
