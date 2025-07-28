namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class StaffKpi
{
    public int KpiId { get; set; }

    public int GoalId { get; set; }

    public string KpiDescription { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual Hrgoal Goal { get; set; } = null!;

    public virtual ICollection<ManagerReview> ManagerReviews { get; set; } = new List<ManagerReview>();
}
