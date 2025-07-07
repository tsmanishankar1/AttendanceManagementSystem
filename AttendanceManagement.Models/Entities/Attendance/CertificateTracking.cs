namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class CertificateTracking
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string FilePath { get; set; } = null!;

    public string FileContent { get; set; } = null!;

    public bool CertificationCourseApplication { get; set; }

    public string CertificationCourse { get; set; } = null!;

    public string Institute { get; set; } = null!;

    public int ValidUpto { get; set; }

    public int YearOfPassing { get; set; }

    public string CourseAppraisal { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
