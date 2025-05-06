namespace AttendanceManagement.Input_Models
{
    public class ApprovePendingStaff
    {
        public bool IsApproved { get; set; }
        public int ApprovedBy { get; set; }
        public IEnumerable<ApproveStaff> SelectedRows { get; set; } = null!;
    }

    public class ApproveStaff
    {
        public int Id { get; set; }
    }
}