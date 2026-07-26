using HRFlow.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Common.Helpers
{
    public static class PermissionHelper
    {
        public static bool CanManageEmployees(ClaimsPrincipal user)
        {
            return user.IsInRole(Roles.Admin) ||
                   user.IsInRole(Roles.HR);
        }

        public static bool CanDeleteEmployee(ClaimsPrincipal user)
        {
            return user.IsInRole(Roles.Admin);
        }

        public static bool CanManageUsers(ClaimsPrincipal user)
        {
            return user.IsInRole(Roles.Admin);
        }

        public static bool CanCreateUser(ClaimsPrincipal user)
        {
            return user.IsInRole(Roles.Admin) ||
                   user.IsInRole(Roles.HR);
        }

        public static bool CanChangeRole(ClaimsPrincipal user)
        {
            return user.IsInRole(Roles.Admin);
        }
    }
}
