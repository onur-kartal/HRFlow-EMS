using HRFlow.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.Payroll
{
    public class PayrollPeriodCreateDto
    {
        [Range(2020, 2100)]
        public int Year { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class PayrollPeriodListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public PayrollPeriodStatus Status { get; set; }

        public int PayrollCount { get; set; }
    }

    public class PayrollPeriodDetailDto : PayrollPeriodListDto
    {
        public List<EmployeePayrollListDto> Payrolls { get; set; } = [];
    }

    public class EmployeePayrollListDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string PositionName { get; set; } = string.Empty;

        public string? ProfileImagePath { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimeAmount { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deduction { get; set; }

        public decimal NetSalary { get; set; }

        public DateTime? PaymentDate { get; set; }

        public EmployeePayrollStatus Status { get; set; }

        public string PeriodName { get; set; } = string.Empty;
    }

    public class EmployeePayrollDetailDto : EmployeePayrollListDto
    {
        public DateTime CreatedDate { get; set; }
    }

    public class EmployeePayrollUpdateDto
    {
        public int Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Bonus { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Deduction { get; set; }

        public DateTime? PaymentDate { get; set; }
    }

    public class MyPayrollListDto : EmployeePayrollListDto
    {
    }
}
