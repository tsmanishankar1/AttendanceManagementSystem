using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class WeeklyOffHolidayWorkingDto
    {
        public int ApplicationTypeId { get; set; }
        [MaxLength(100)]
        public string SelectShiftType { get; set; } = null!;
        public DateOnly TxnDate { get; set; }
        public int ShiftId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public int? StaffId { get; set; }
        public int CreatedBy { get; set; }
    }
}