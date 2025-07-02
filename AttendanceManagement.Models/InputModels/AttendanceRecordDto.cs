using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class AttendanceRecordDto
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public DateTime? FirstIn { get; set; }
        public DateTime? LastOut { get; set; }
        public int? ShiftId { get; set; }
        public bool? IsEarlyComing { get; set; }
        public bool? IsLateComing { get; set; }
        public bool? IsEarlyGoing { get; set; }
        public bool? IsLateGoing { get; set; }
        public decimal? BreakHours { get; set; }
        public bool? IsBreakHoursExceed { get; set; }
        public decimal? ExtraBreakHours { get; set; }
        public int StatusId { get; set; }
        public bool IsHolidayWorkingEligible { get; set; }
        public int? Norm {  get; set; }
        public int? CompletedFileCouunt {  get; set; }
        public decimal? TotalFte { get; set; }
        public bool? IsFteAchieved {  get; set; }
        public DateOnly? AttendanceDate { get; set; }
    }

    public class AttendanceRecordResponse
    {
        public int Id { get; set; }
        public decimal? BreakHour { get; set; }
        public bool? IsBreakHoursExceed { get; set; }
        public decimal? ExtraBreakHours { get; set; }
        public DateTime? FirstIn { get; set; }
        public DateTime? LastOut { get; set; }
        public bool? IsEarlyComing { get; set; }
        public bool? IsLateComing { get; set; }
        public bool? IsEarlyGoing { get; set; }
        public bool? IsLateGoing { get; set; }
        public int? ShiftId { get; set; }
        public int StaffId { get; set; }
        public bool? IsRegularized { get; set; }
        public int StatusId { get; set; }
        public bool IsHolidayWorkingEligible { get; set; }
        public int? Norm { get; set; }
        public int? CompletedFileCount { get; set; }
        public decimal? TotalFte { get; set; }
        public bool? IsFteAchieved { get; set; }
        public bool? IsFreezed { get; set; }
        public int? FreezedBy { get; set; }
        public DateTime? FreezedOn { get; set; }
        public DateOnly AttendanceDate { get; set; }
    }

    public class AttendanceStatusColorResponse
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string? ColourCode { get; set; }
        public bool IsActive {  get; set; }

    }

    public class UpdateAttendanceStatusColor
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string StatusName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(100)]
        public string ColourCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }
}