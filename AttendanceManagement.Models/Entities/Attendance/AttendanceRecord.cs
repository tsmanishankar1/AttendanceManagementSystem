namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class AttendanceRecord
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

    public bool? IsFreezed { get; set; }

    public bool IsDeleted { get; set; }

    public int? ShiftId { get; set; }

    public int StaffId { get; set; }

    public bool? IsRegularized { get; set; }

    public int StatusId { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? FreezedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsHolidayWorkingEligible { get; set; }

    public int? Norm { get; set; }

    public int? CompletedFileCount { get; set; }

    public decimal? TotalFte { get; set; }

    public bool? IsFteAchieved { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public DateTime? FreezedOn { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? FreezedByNavigation { get; set; }

    public virtual AssignShift? Shift { get; set; }

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StatusDropdown Status { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
