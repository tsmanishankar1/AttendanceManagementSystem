namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class TeamApplication
{
    public int Id { get; set; }

    public int UserManagementId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
