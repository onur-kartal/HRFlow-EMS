using HRFlow.Entities.HumanResources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Entities.Identity
{
    public class SystemUser : IdentityUser
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public DateTime? LastLoginDate { get; set; }
    }
}
