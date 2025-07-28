using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class CommonPermission
{
    public int Id { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string TotalHours { get; set; } = null!;

    public DateOnly PermissionDate { get; set; }

    public string PermissionType { get; set; } = null!;

    public bool? Status1 { get; set; }

    public string Remarks { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? ApplicationTypeId { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public int? StaffId { get; set; }

    public bool? IsCancelled { get; set; }

    public DateTime? CancelledOn { get; set; }

    public bool? Status2 { get; set; }

    public bool? IsEmailSent { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public int? CancelledBy { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public virtual ApplicationType? ApplicationType { get; set; }

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation? ApprovedByNavigation { get; set; }

    public virtual StaffCreation? CancelledByNavigation { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
