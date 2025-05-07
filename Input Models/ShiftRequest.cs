using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class ShiftRequest
    {
        [MaxLength(100)]
        public string ShiftName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int ShiftTypeId { get; set; }
        [MaxLength(20)]
        public string StartTime { get; set; } = null!;
        [MaxLength(20)]
        public string EndTime { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class StaffsDto
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string DesignationName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
    }

    public class AssignShiftRequest
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int ShiftId { get; set; }
        public int CreatedBy { get; set; }
        public IEnumerable<ApproveLeave> selectedRows { get; set; } = null!;
    }

    public class AssignedShiftResponse
    {
        public string ShiftName { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string StaffName { get; set; } = null!;
    }

    public class ShiftResponse
    {
        public int ShiftId { get; set; }
        public int ShiftTypeId { get; set; }
        public string ShiftTypeName { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateShift
    {
        public int ShiftId { get; set; }
        [MaxLength(100)]
        public string ShiftName { get; set; } = null!;
        public int ShiftTypeId { get; set; }
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        [MaxLength(20)]
        public string StartTime { get; set; } = null!;
        [MaxLength(20)]
        public string EndTime { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class RegularShiftRequest
    {
        [MaxLength(100)]
        public string ShiftType { get; set; } = null!;
        public bool? WeeklyOffType { get; set; }
        [MaxLength(100)]
        public string? DayPattern { get; set; } = null!;
        public DateOnly ChangeEffectFrom { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; } = null!;
        public int? ShiftId { get; set; }
        [MaxLength(255)]
        public string? ShiftPattern { get; set; }
        public List<int> StaffIds { get; set; } = new List<int>();  
        public int CreatedBy { get; set; }
    }
}