using HRFlow.Business.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginDto model);

        Task LogoutAsync();

        Task<bool> CreateUserFromEmployeeAsync(int employeeId);

        Task<string?> GetUserRoleAsync(int employeeId);

        Task<ChangeRoleDto?> GetChangeRoleDtoAsync(int employeeId);

        Task<bool> ChangeRoleAsync(ChangeRoleDto model);

        Task<ProfileDto?> GetProfileAsync();

        Task<bool> UpdateProfileAsync(ProfileUpdateDto model);

        Task<Microsoft.AspNetCore.Identity.IdentityResult> ChangePasswordAsync(ChangePasswordDto model);
    }
}
