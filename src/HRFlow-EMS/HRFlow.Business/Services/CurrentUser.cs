using HRFlow.Business.Interfaces;
using HRFlow.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<SystemUser> _userManager;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<SystemUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public string? UserId =>
            User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? UserName =>
            User?.Identity?.Name;

        public string? Email =>
            User?.FindFirstValue(ClaimTypes.Email);

        public string? IpAddress =>
            _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        public int EmployeeId
        {
            get
            {
                if (!IsAuthenticated)
                    return 0;

                var user = _userManager.Users
                    .FirstOrDefault(x => x.Id == UserId);

                return user?.EmployeeId ?? 0;
            }
        }

        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }
    }
}
