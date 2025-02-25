namespace AttendanceManagement.Input_Models
{
    public class OnDutyRequisitionRequest
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Reason { get; set; } = null!;
        public string? TotalHours { get; set; }
        public decimal? TotalDays { get; set; }
        public int CreatedBy { get; set; }
    }
}