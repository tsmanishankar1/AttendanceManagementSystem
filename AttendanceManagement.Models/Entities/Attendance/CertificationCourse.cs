namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class CertificationCourse
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public string? Course { get; set; }

    public DateOnly? ValidUpto { get; set; }

    public string? CourseAppraisal { get; set; }

    public string? CertificationInstitute { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
