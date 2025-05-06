namespace AttendanceManagement.Input_Models
{
    public class LeaveTypeRequest
    {
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool Accountable { get; set; }
        public bool Encashable { get; set; }
        public bool PaidLeave { get; set; }
        public bool CommonType { get; set; }
        public bool PermissionType { get; set; }
        public bool CarryForward { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class LeaveTypeResponse
    {
        public int LeaveTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool Accountable { get; set; }
        public bool Encashable { get; set; }
        public bool PaidLeave { get; set; }
        public bool CommonType { get; set; }
        public bool PermissionType { get; set; }
        public bool CarryForward { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateLeaveType
    {
        public int LeaveTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;  
        public bool Accountable { get; set; }
        public bool Encashable { get; set; }
        public bool PaidLeave { get; set; }
        public bool CommonType { get; set; }
        public bool PermissionType { get; set; }
        public bool CarryForward { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}