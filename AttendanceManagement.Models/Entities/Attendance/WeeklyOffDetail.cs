namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class WeeklyOffDetail
{
    public int Id { get; set; }

    public int WeeklyOffMasterId { get; set; }

    public int MarkWeeklyOff { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual WeeklyOffMaster WeeklyOffMaster { get; set; } = null!;
}
