namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class FeedbackRequest
    {
        public int ProbationId { get; set; }
        public string FeedbackText { get; set; } = null!;
        public DateOnly? ExtensionPeriod { get; set; }
        public bool IsApproved { get; set; }
        public int CreatedBy { get; set; }
    }

    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int ProbationId { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName{ get; set; } = null!;
        public string? FeedbackText { get; set; }
        public DateOnly? ProbationExtensionPeriod { get; set; }
        public bool? IsApproved { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateFeedback
    {
        public int FeedbackId { get; set; }
        public int ProbationId { get; set; }
        public string FeedbackText { get; set; } = null!;
        public int UpdatedBy { get; set; }
    }
}