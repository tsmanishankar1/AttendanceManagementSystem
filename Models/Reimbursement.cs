using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class Reimbursement
{
    public int Id { get; set; }

    public DateOnly BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string BillPeriod { get; set; } = null!;

    public decimal Amount { get; set; }

    public string UploadFilePath { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public int? StaffId { get; set; }

    public int ReimbursementTypeId { get; set; }

    public bool? Status1 { get; set; }

    public bool? Status2 { get; set; }

    public DateTime? CancelledOn { get; set; }

    public int? ApplicationTypeId { get; set; }

    public bool? IsCancelled { get; set; }

    public int? ApprovalNotificationId { get; set; }

    public bool? IsEmailSent { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public virtual ApplicationType? ApplicationType { get; set; }

    public virtual ApprovalNotification? ApprovalNotification { get; set; }

    public virtual StaffCreation? ApprovedByNavigation { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ReimbursementType ReimbursementType { get; set; } = null!;

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
