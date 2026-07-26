using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Account
{
    public class ChangeRoleDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeFullName { get; set; } = string.Empty;

        public string CurrentRole { get; set; } = string.Empty;

        public string SelectedRole { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = [];
    }
}
