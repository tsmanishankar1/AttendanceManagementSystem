namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class ManagerReview
{
    public int ReviewId { get; set; }

    public int KpiId { get; set; }

    public string? Comments { get; set; }

    public int Rating { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Hrapproval> Hrapprovals { get; set; } = new List<Hrapproval>();

    public virtual StaffKpi Kpi { get; set; } = null!;
}
