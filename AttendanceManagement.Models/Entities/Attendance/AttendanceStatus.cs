namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class AttendanceStatus
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int StatusId { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? DurationId { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffLeaveOption? Duration { get; set; }

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StatusDropdown Status { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
