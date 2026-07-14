using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;

namespace HRFlow.Web.Models.Department
{
    public class DepartmentEditViewModel
    {
        public DepartmentUpdateDto Department { get; set; } = new();
    }
}
