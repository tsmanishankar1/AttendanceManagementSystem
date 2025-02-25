namespace AttendanceManagement.Input_Models
{
    public class ApprovalRequest
    {
        public int FeedbackId { get; set; }

        public bool? IsApproved { get; set; }

        public string? ApprovalComment { get; set; }

        public int CreatedBy { get; set; }
    }
}
