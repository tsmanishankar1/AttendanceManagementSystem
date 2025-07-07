using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class AddLeaveGroupDto
    {
        [MaxLength(255)]
        public string LeaveGroupName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public List<int> LeaveTypeIds { get; set; } = new List<int>();
    }

    public class UpdateLeaveGroup
    {
        public int LeaveGroupId { get; set; }
        [MaxLength(255)]
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