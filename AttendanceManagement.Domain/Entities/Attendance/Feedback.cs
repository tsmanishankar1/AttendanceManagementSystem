using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Feedback
{
    public int Id { get; set; }

    public int ProbationId { get; set; }

    public string FeedbackText { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public bool? IsApproved { get; set; }

    public DateOnly? ExtensionPeriod { get; set; }

    public bool? IsNotificationSent { get; set; }

    public virtual ICollection<Approval> Approvals { get; set; } = new List<Approval>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual Probation Probation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
