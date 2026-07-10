using HRFlow.Business.DTOs.Employee;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRFlow.Web.Models.Employee
{
    public class EmployeeEditViewModel
    {
        public EmployeeUpdateDto Employee { get; set; } = new();

        public List<SelectListItem> Departments { get; set; } = new();

        public List<SelectListItem> Positions { get; set; } = new();
    }
}
