namespace AttendanceManagement.Input_Models
{
    public class ApproveLeaveRequest
    {
        public bool IsApproved { get; set; }
        public int ApprovedBy { get; set; }
        public int ApplicationTypeId { get; set; }
        public IEnumerable<ApproveLeave> SelectedRows { get; set; } = null!;
    }

    public class ApproveLeave
    {
        public int Id { get; set; }
    }
}
