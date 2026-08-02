using HRFlow.Entities.Base;
using HRFlow.Entities.Enums;

namespace HRFlow.Entities.HumanResources
{
    public class EmployeePayroll : BaseEntity
    {
        public int PayrollPeriodId { get; set; }

        public PayrollPeriod PayrollPeriod { get; set; } = null!;

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public decimal BaseSalary { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimeAmount { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deduction { get; set; }

        public decimal NetSalary { get; set; }

        public DateTime? PaymentDate { get; set; }

        public EmployeePayrollStatus Status { get; set; }
    }
}
