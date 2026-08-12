using HRFlow.Common.Enums;
using HRFlow.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HRFlow.Web.Helpers
{
    public static class StatusDisplayHelper
    {
        public static string GetDisplayText(Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? value.ToString();
        }

        public static string GetText(LeaveStatus status)
        {
            return status switch
            {
                LeaveStatus.Pending => "Bekliyor",
                LeaveStatus.Approved => "Onaylandı",
                LeaveStatus.Rejected => "Reddedildi",
                LeaveStatus.Cancelled => "İptal Edildi",
                _ => status.ToString()
            };
        }

        public static string GetText(OvertimeStatus status)
        {
            return status switch
            {
                OvertimeStatus.Pending => "Bekliyor",
                OvertimeStatus.Approved => "Onaylandı",
                OvertimeStatus.Rejected => "Reddedildi",
                OvertimeStatus.Cancelled => "İptal Edildi",
                _ => status.ToString()
            };
        }

        public static string GetText(PayrollPeriodStatus status)
        {
            return status switch
            {
                PayrollPeriodStatus.Draft => "Taslak",
                PayrollPeriodStatus.Approved => "Onaylandı",
                PayrollPeriodStatus.Paid => "Ödendi",
                _ => status.ToString()
            };
        }

        public static string GetText(EmployeePayrollStatus status)
        {
            return status switch
            {
                EmployeePayrollStatus.Draft => "Taslak",
                EmployeePayrollStatus.Approved => "Onaylandı",
                EmployeePayrollStatus.Paid => "Ödendi",
                _ => status.ToString()
            };
        }
    }
}
