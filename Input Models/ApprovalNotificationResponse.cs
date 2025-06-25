namespace AttendanceManagement.Input_Models
{
    public class ApprovalNotificationResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string Message { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class RequestNotificationResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string Message { get; set; } = null!;
        public int? ApplicationTypeId { get; set; }
        public int PendingCount { get; set; }
        public int CreatedBy { get; set; }
    }

}