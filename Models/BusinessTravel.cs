using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class BusinessTravel
{
    public int Id { get; set; }

    public int ApplicationTypeId { get; set; }

    public DateTime? FromTime { get; set; }

    public DateTime? ToTime { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string Reason { get; set; } = null!;

    public bool? Status1 { get; set; }

    public bool? Status2 { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public decimal? TotalDays { get; set; }

    public string? TotalHours { get; set; }

    public string StartDuration { get; set; } = null!;

    public string? EndDuration { get; set; }

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
