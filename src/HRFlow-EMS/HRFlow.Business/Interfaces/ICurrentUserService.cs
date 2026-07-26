using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }

        int EmployeeId { get; }

        string? UserName { get; }

        string? Email { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}
