using System.Text.RegularExpressions;

namespace HRFlow.Web.Helpers
{
    public sealed record PageMetadata(string Title, string? Section, bool IsDashboard = false);

    public static class PageMetadataHelper
    {
        private static readonly Dictionary<string, PageMetadata> PageMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Dashboard:Index"] = new("Dashboard", null, true),
            ["Employee:Index"] = new("Employees", "Employee Management"),
            ["Employee:Create"] = new("New Employee", "Employee Management"),
            ["Employee:Edit"] = new("Edit Employee", "Employee Management"),
            ["Employee:Details"] = new("Employee Details", "Employee Management"),
            ["Employee:ChangeRole"] = new("Change Employee Role", "Employee Management"),
            ["Employee:Delete"] = new("Employees", "Employee Management"),
            ["Department:Index"] = new("Departments", "Organization"),
            ["Department:Create"] = new("New Department", "Organization"),
            ["Department:Edit"] = new("Edit Department", "Organization"),
            ["Department:Delete"] = new("Departments", "Organization"),
            ["Position:Index"] = new("Positions", "Organization"),
            ["Position:Create"] = new("New Position", "Organization"),
            ["Position:Edit"] = new("Edit Position", "Organization"),
            ["Position:Delete"] = new("Positions", "Organization"),
            ["LeaveRequest:Index"] = new("Leave Requests", "Leave Management"),
            ["LeaveRequest:Create"] = new("New Leave Request", "Leave Management"),
            ["LeaveRequest:Edit"] = new("Edit Leave Request", "Leave Management"),
            ["LeaveRequest:Approve"] = new("Approve Leave Request", "Leave Management"),
            ["LeaveRequest:Reject"] = new("Reject Leave Request", "Leave Management"),
            ["LeaveRequest:Cancel"] = new("Cancel Leave Request", "Leave Management"),
            ["LeaveType:Index"] = new("Leave Types", "Leave Management"),
            ["LeaveType:Create"] = new("New Leave Type", "Leave Management"),
            ["LeaveType:Edit"] = new("Edit Leave Type", "Leave Management"),
            ["LeaveType:Delete"] = new("Leave Types", "Leave Management"),
            ["OvertimeRequest:MyRequests"] = new("My Overtime Requests", "Overtime Management"),
            ["OvertimeRequest:Create"] = new("New Overtime Request", "Overtime Management"),
            ["OvertimeRequest:ApprovalList"] = new("Overtime Approvals", "Overtime Management"),
            ["OvertimeRequest:Management"] = new("Overtime Requests", "Overtime Management"),
            ["OvertimeRequest:Approve"] = new("Approve Overtime Request", "Overtime Management"),
            ["OvertimeRequest:Reject"] = new("Reject Overtime Request", "Overtime Management"),
            ["OvertimeRequest:Cancel"] = new("Cancel Overtime Request", "Overtime Management"),
            ["OvertimeRequest:ChangeStatus"] = new("Update Overtime Status", "Overtime Management"),
            ["Announcement:Index"] = new("Announcements", "Announcement Management"),
            ["Announcement:Create"] = new("New Announcement", "Announcement Management"),
            ["Announcement:Edit"] = new("Edit Announcement", "Announcement Management"),
            ["Announcement:Delete"] = new("Announcements", "Announcement Management"),
            ["Announcement:ChangeStatus"] = new("Update Announcement Status", "Announcement Management"),
            ["PayrollPeriod:Index"] = new("Payroll Periods", "Payroll Management"),
            ["PayrollPeriod:Create"] = new("New Payroll Period", "Payroll Management"),
            ["PayrollPeriod:Details"] = new("Payroll Period Details", "Payroll Management"),
            ["PayrollPeriod:GeneratePayrolls"] = new("Generate Payrolls", "Payroll Management"),
            ["PayrollPeriod:Approve"] = new("Approve Payroll Period", "Payroll Management"),
            ["PayrollPeriod:RevertApproval"] = new("Revert Payroll Approval", "Payroll Management"),
            ["PayrollPeriod:MarkAsPaid"] = new("Mark Payroll Period as Paid", "Payroll Management"),
            ["PayrollPeriod:ChangeStatus"] = new("Update Payroll Period Status", "Payroll Management"),
            ["EmployeePayroll:Index"] = new("Employee Payroll", "Payroll Management"),
            ["EmployeePayroll:Details"] = new("Payroll Details", "Payroll Management"),
            ["EmployeePayroll:Edit"] = new("Edit Payroll", "Payroll Management"),
            ["EmployeePayroll:Approve"] = new("Approve Payroll", "Payroll Management"),
            ["EmployeePayroll:MarkAsPaid"] = new("Mark Payroll as Paid", "Payroll Management"),
            ["EmployeePayroll:MyPayrolls"] = new("My Payrolls", "Payroll"),
            ["EmployeePayroll:MyDetails"] = new("Payroll Details", "Payroll"),
            ["EmployeePayroll:ViewPdf"] = new("Payroll Details", "Payroll"),
            ["EmployeePayroll:DownloadPdf"] = new("Payroll Details", "Payroll"),
            ["AuditLog:Index"] = new("Audit Logs", "Audit"),
            ["RequestLog:Index"] = new("Request Logs", "Audit"),
            ["Account:Profile"] = new("My Profile", "Profile"),
            ["Account:UpdateProfile"] = new("My Profile", "Profile"),
            ["Account:ChangePassword"] = new("My Profile", "Profile"),
            ["Account:Login"] = new("Sign In", "Account"),
            ["Account:Logout"] = new("Sign Out", "Account"),
            ["Account:AccessDenied"] = new("Access Denied", "Account"),
            ["Notification:Index"] = new("Bildirimlerim", null),
            ["Notification:Open"] = new("Bildirimlerim", null),
            ["Home:Index"] = new("Home", null),
            ["Home:Privacy"] = new("Privacy", null),
            ["Home:Error"] = new("Error", null)
        };

        public static PageMetadata Resolve(string? controller, string? action, string? requestedTitle)
        {
            var normalizedController = string.IsNullOrWhiteSpace(controller) ? "Home" : controller;
            var normalizedAction = string.IsNullOrWhiteSpace(action) ? "Index" : action;

            if (PageMappings.TryGetValue($"{normalizedController}:{normalizedAction}", out var mappedPage))
            {
                return string.IsNullOrWhiteSpace(requestedTitle)
                    ? mappedPage
                    : mappedPage with { Title = requestedTitle };
            }

            var controllerTitle = SplitPascalCase(normalizedController);
            var actionTitle = SplitPascalCase(normalizedAction);
            var title = string.IsNullOrWhiteSpace(requestedTitle)
                ? normalizedAction.Equals("Index", StringComparison.OrdinalIgnoreCase)
                    ? controllerTitle
                    : actionTitle
                : requestedTitle;

            return new PageMetadata(title, controllerTitle);
        }

        private static string SplitPascalCase(string value)
        {
            return Regex.Replace(value, "(?<!^)([A-Z])", " $1");
        }
    }
}
