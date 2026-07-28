using HRFlow.Business.DTOs.Account;

namespace HRFlow.Web.Models.Account
{
    public class ProfileViewModel
    {
        public ProfileDto Profile { get; set; } = new();

        public ChangePasswordDto ChangePassword { get; set; } = new();
        public ProfileUpdateDto ProfileUpdate { get; set; } = new();
    }
}
