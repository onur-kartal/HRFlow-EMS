using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Employee
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; } = string.Empty;
        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        [EmailAddress] public string? PersonalEmail { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(500)] public string? Address { get; set; }
        [StringLength(100)] public string? City { get; set; }
        [StringLength(100)] public string? District { get; set; }
        [StringLength(10)] public string? PostalCode { get; set; }
        public string? ProfileImagePath { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Departman seçiniz.")]
        public int DepartmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Pozisyon seçiniz.")]
        public int PositionId { get; set; }
    }
}
