using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.Account
{
    public class ProfileUpdateDto
    {
        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? PersonalEmail { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }
    }
}
