namespace HRFlow.Business.DTOs.Logging
{
    public class RequestLogListDto
    {
        public DateTime CreatedDate { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string HttpMethod { get; set; } = string.Empty;
        public string RequestPath { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string? IpAddress { get; set; }
        public string? Browser { get; set; }
    }
}
