namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class UserVsRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual AccessLevel Role { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual StaffCreation User { get; set; } = null!;
}
