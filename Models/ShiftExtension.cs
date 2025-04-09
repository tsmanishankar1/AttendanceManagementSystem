using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ShiftExtension
{
    public int Id { get; set; }

    public int ApplicationTypeId { get; set; }

    public DateOnly TransactionDate { get; set; }

    public string? DurationHours { get; set; }

    public DateTime? BeforeShiftHours { get; set; }

    public DateTime? AfterShiftHours { get; set; }

    public string? Remarks { get; set; }

    public bool? Status1 { get; set; }

    public bool? Status2 { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int? StaffId { get; set; }

    public bool? IsCancelled { get; set; }

    public int ShiftId { get; set; }

    public DateTime? CancelledOn { get; set; }

    public bool? IsEmailSent { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation? ApprovedByNavigation { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual Shift Shift { get; set; } = null!;

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
