namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class GraceTimeDropdown
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<AttendanceGraceTimeCalc> AttendanceGraceTimeCalcs { get; set; } = new List<AttendanceGraceTimeCalc>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
