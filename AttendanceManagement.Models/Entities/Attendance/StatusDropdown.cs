namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class StatusDropdown
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string ShortName { get; set; } = null!;

    public int ColorCodeId { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<AttendanceStatus> AttendanceStatuses { get; set; } = new List<AttendanceStatus>();

    public virtual AttendanceStatusColor ColorCode { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
