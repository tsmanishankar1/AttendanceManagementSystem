namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ApprovalRequest
    {
        public int FeedbackId { get; set; }
        public bool? IsApproved { get; set; }
        public string? ApprovalComment { get; set; }
        public int CreatedBy { get; set; }
    }

    public class HrConfirmation
    {
        public int ProbationId { get; set; }
        public bool IsCompleted { get; set; }
        public int CreatedBy { get; set; }
    }
}