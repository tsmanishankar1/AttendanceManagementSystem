using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class Probation
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public DateOnly ProbationStartDate { get; set; }

    public DateOnly ProbationEndDate { get; set; }

    public bool? IsCompleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int ManagerId { get; set; }

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual StaffCreation Manager { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
