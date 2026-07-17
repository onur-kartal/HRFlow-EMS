using HRFlow.Business.DTOs.LeaveRequest;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRFlow.Web.Models.LeaveRequest
{
    public class LeaveRequestUpdateViewModel
    {
        public LeaveRequestUpdateDto LeaveRequest { get; set; } = new();

        public List<SelectListItem> Employees { get; set; } = new();

        public List<SelectListItem> LeaveTypes { get; set; } = new();
    }
}
