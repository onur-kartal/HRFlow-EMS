using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Employee
{
    public class EmployeeLookupDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}
