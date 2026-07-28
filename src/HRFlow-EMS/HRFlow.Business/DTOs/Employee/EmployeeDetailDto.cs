namespace HRFlow.Business.DTOs.Employee
{
    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PersonalEmail { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? PostalCode { get; set; }
        public DateTime HireDate { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
    }
}
