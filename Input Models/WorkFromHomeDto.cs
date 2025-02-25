namespace AttendanceManagement.Input_Models
{
    public class WorkFromHomeDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string Reason { get; set; } = null!;
        public string? TotalHours { get; set; }
        public decimal? TotalDays { get; set; }
        public int CreatedBy { get; set; }
    }
}
