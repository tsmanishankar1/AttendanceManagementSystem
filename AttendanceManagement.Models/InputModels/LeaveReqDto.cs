namespace AttendanceManagement.InputModels
{
    public class LeaveReqDto
    {
        public int LeaveRequestId { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal? TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public bool? Status1 { get; set; }
        public bool? Status2 { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string ApplicationTypeName { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}