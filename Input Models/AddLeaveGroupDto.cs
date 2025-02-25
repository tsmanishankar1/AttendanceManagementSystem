namespace AttendanceManagement.Input_Models
{
    public class AddLeaveGroupDto
    {
        public string LeaveGroupName { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        public List<int> LeaveTypeIds { get; set; } = new List<int>();
    }

    public class UpdateLeaveGroup
    {
        public int LeaveGroupId { get; set; }

        public string? LeaveGroupName { get; set; }
        public List<int> LeaveTypeIds { get; set; } = new List<int>();
        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }

    public class LeaveGroupResponse
    {
        public int LeaveGroupId { get; set; }
        public List<int> LeaveTypeIds {  get; set; } = new List<int>();
        public string? LeaveGroupName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

}
