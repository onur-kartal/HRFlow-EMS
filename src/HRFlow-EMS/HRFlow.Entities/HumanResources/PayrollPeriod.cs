using HRFlow.Entities.Base;
using HRFlow.Entities.Enums;

namespace HRFlow.Entities.HumanResources
{
    public class PayrollPeriod : BaseEntity
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public PayrollPeriodStatus Status { get; set; }

        public List<EmployeePayroll> EmployeePayrolls { get; set; } = [];
    }
}
