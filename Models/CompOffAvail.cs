using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class CompOffAvail
{
    public int Id { get; set; }

    public int ApplicationTypeId { get; set; }

    public DateOnly WorkedDate { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string FromDuration { get; set; } = null!;

    public string ToDuration { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public decimal TotalDays { get; set; }

    public bool? Status1 { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int? StaffId { get; set; }

    public bool? IsCancelled { get; set; }

    public DateTime? CancelledOn { get; set; }

    public bool? Status2 { get; set; }

    public bool? IsEmailSent { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation? ApprovedByNavigation { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
