using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class PrefixAndSuffixDto
    {
        public int PrefixAndSuffixId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public string PrefixName { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public int SuffixTypeId { get; set; }
        public int PrefixTypeId { get; set; }
        public string SuffixName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class PrefixAndSuffixRequest
    {
        public int PrefixTypeId { get; set; }
        public int SuffixTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int LeaveTypeId { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdatePrefixAndSuffix
    {
        public int PrefixAndSuffixId { get; set; }
        public int PrefixTypeId { get; set; }
        public int SuffixTypeId { get; set; }
        public int UpdatedBy { get; set; }
        public int LeaveTypeId { get; set; }
        public bool IsActive { get; set; }
    }

    public class SuffixLeaveRequest
    {
        [MaxLength(50)]
        public string SuffixLeaveTypeName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SuffixLeaveResponse
    {
        public int SuffixLeaveTypeId { get; set; }
        public string SuffixLeaveTypeName { get; set; } = null!;
    }

    public class PrefixLeaveRequest
    {
        [MaxLength(50)]
        public string PrefixLeaveTypeName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class PrefixLeaveResponse
    {
        public int PrefixLeaveTypeId { get; set; }
        public string PrefixLeaveTypeName { get; set; } = null!;
    }
}