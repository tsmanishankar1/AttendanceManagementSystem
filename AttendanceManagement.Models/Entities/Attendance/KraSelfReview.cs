namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class KraSelfReview
{
    public int Id { get; set; }

    public int GoalId { get; set; }

    public int SelfEvaluationScale { get; set; }

    public decimal SelfScore { get; set; }

    public string SelfEvaluationComments { get; set; } = null!;

    public string? AttachmentsSelf { get; set; }

    public int AppraisalId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public DateTime? CompletedUtc { get; set; }

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual Goal Goal { get; set; } = null!;

    public virtual ICollection<KraManagerReview> KraManagerReviews { get; set; } = new List<KraManagerReview>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
