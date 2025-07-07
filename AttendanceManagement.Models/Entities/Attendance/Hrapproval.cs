namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Hrapproval
{
    public int ApprovalId { get; set; }

    public int ReviewId { get; set; }

    public string Status { get; set; } = null!;

    public string? Comments { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ManagerReview Review { get; set; } = null!;
}
