using HRFlow.Business.DTOs.Dashboard;

namespace HRFlow.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public DashboardDto? AdminHrDashboard { get; set; }
        public ManagerDashboardDto? ManagerDashboard { get; set; }
        public EmployeeDashboardDto? EmployeeDashboard { get; set; }
    }
}
