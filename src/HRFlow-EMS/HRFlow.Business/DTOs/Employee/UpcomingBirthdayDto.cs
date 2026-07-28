namespace HRFlow.Business.DTOs.Employee
{
    public class UpcomingBirthdayDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? ProfileImagePath { get; set; }
        public int DaysLeft { get; set; }
    }
}
