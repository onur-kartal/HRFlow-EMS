using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Dashboard
{
    public class DepartmentChartDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public int EmployeeCount { get; set; }
    }
}
