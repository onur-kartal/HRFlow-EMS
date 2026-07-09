using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Employee
{
    public class EmployeeListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string PositionName { get; set; } = string.Empty;
    }
}
