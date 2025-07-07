namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Probation
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public DateOnly ProbationStartDate { get; set; }

    public DateOnly ProbationEndDate { get; set; }

    public bool? IsCompleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int? ManagerId { get; set; }

    public int? AssignedBy { get; set; }

    public DateTime? AssignedOn { get; set; }

    public bool? IsAssigned { get; set; }

    public bool? IsNotificationSent { get; set; }

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation? AssignedByNavigation { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual StaffCreation? Manager { get; set; }

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
