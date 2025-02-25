namespace AttendanceManagement.Input_Models
{
    public class ApprovalNotificationResponse
    {
        public int Id { get; set; }

        public int ApplicationTypeId { get; set; }

        public int StaffId { get; set; }

        public string Message { get; set; } = null!;

        public int CreatedBy { get; set; }
    }
}
