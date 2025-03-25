using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class CompOffCredit
{
    public int Id { get; set; }

    public int ApplicationTypeId { get; set; }

    public DateOnly WorkedDate { get; set; }

    public int? Balance { get; set; }

    public int TotalDays { get; set; }

    public string Reason { get; set; } = null!;

    public bool? Status { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int? StaffId { get; set; }

    public bool? IsCancelled { get; set; }

    public DateTime? CancelledOn { get; set; }

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
