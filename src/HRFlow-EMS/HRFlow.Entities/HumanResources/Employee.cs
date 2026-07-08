using HRFlow.Entities.Base;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Entities.HumanResources
{
    public class Employee : BaseEntity
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        public int PositionId { get; set; }

        public Position Position { get; set; } = null!;
    }
}
