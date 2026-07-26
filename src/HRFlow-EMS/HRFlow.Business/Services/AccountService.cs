using HRFlow.Business.DTOs.Account;
using HRFlow.Business.Interfaces;
using HRFlow.Data.Context;
using HRFlow.Entities.Identity;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly UserManager<SystemUser> _userManager;
        private readonly HRFlowDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICurrentUserService _currentUser;
        private readonly AutoMapper.IMapper _mapper;
        private readonly IAuditLogService _auditLogService;

        public AccountService(
            SignInManager<SystemUser> signInManager,
            UserManager<SystemUser> userManager,
            HRFlowDbContext context,
            RoleManager<IdentityRole> roleManager,
            ICurrentUserService currentUser,
            AutoMapper.IMapper mapper,
            IAuditLogService auditLogService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _currentUser = currentUser;
            _mapper = mapper;
            _auditLogService = auditLogService;
        }

        public async Task<bool> ChangeRoleAsync(ChangeRoleDto model)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);

            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                    return false;
            }

            var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);

            if (addResult.Succeeded)
                await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.Role, Action = AuditAction.RoleChanged, EntityId = model.EmployeeId, Description = "Kullanıcı rolü değiştirildi." });
            return addResult.Succeeded;
        }

        public async Task<bool> CreateUserFromEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
                return false;

            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            if (existingUser != null)
                return false;

            var user = new SystemUser
            {
                EmployeeId = employee.Id,
                UserName = employee.Email,
                Email = employee.Email,
                LastLoginDate = null
            };

            var result = await _userManager.CreateAsync(user, "HrFlow@123");

            if (!result.Succeeded)
                return false;

            await _userManager.AddToRoleAsync(user, "Employee");

            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.User, Action = AuditAction.Created, EntityId = employeeId, Description = "Çalışan için kullanıcı hesabı oluşturuldu." });
            return true;
        }

        public async Task<ChangeRoleDto?> GetChangeRoleDtoAsync(int employeeId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
                return null;

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            if (user == null)
                return null;

            var currentRole = (await _userManager.GetRolesAsync(user))
                .FirstOrDefault();

            var roles = await _roleManager.Roles
                .Select(x => x.Name!)
                .ToListAsync();

            return new ChangeRoleDto
            {
                EmployeeId = employee.Id,

                EmployeeFullName = employee.FirstName + " " + employee.LastName,

                CurrentRole = currentRole ?? "",

                SelectedRole = currentRole ?? "",

                Roles = roles
            };
        }

        public async Task<string?> GetUserRoleAsync(int employeeId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        public async Task<bool> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.Authentication, Action = AuditAction.LoginFailed, UserName = model.Email, Description = "Başarısız giriş denemesi." });
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.Authentication, Action = result.Succeeded ? AuditAction.LoginSuccess : AuditAction.LoginFailed, UserId = user.Id, EmployeeId = user.EmployeeId, UserName = user.UserName, Description = result.Succeeded ? "Başarılı giriş." : "Başarısız giriş denemesi." });
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.Authentication, Action = AuditAction.Logout, Description = "Kullanıcı çıkış yaptı." });
        }

        public async Task<ProfileDto?> GetProfileAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentUser.UserId))
                return null;

            var user = await _userManager.Users
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Department)
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == _currentUser.UserId);

            if (user == null)
                return null;

            var profile = _mapper.Map<ProfileDto>(user);
            var roles = await _userManager.GetRolesAsync(user);

            profile.RoleName = roles.FirstOrDefault() ?? string.Empty;

            return profile;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto model)
        {
            if (string.IsNullOrWhiteSpace(_currentUser.UserId))
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Giriş yapan kullanıcı bulunamadı."
                });

            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Giriş yapan kullanıcı bulunamadı."
                });

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword);
            if (result.Succeeded)
                await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.Authentication, Action = AuditAction.PasswordChanged, Description = "Parola değiştirildi." });
            return result;
        }
    }
}
