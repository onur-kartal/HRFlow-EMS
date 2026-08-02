using HRFlow.Business.DTOs.Payroll;

namespace HRFlow.Web.Services
{
    public interface IPayrollPdfService
    {
        byte[] Generate(EmployeePayrollDetailDto payroll);
    }
}
