namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class DivisionMaster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PerformanceReport> PerformanceReports { get; set; } = new List<PerformanceReport>();

    public virtual ICollection<ProbationTarget> ProbationTargets { get; set; } = new List<ProbationTarget>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
