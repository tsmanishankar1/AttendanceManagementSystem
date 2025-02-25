using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveApplication
{
    public string Id { get; set; } = null!;

    public string? StaffId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int DurationId { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime LeaveStartDate { get; set; }

    public DateTime LeaveEndDate { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string LeaveReason { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<AbsenceApproval> AbsenceApprovals { get; set; } = new List<AbsenceApproval>();

    public virtual Staff? CreatedByNavigation { get; set; }

    public virtual LeaveDuration Duration { get; set; } = null!;

    public virtual LeaveType? LeaveType { get; set; }

    public virtual Staff? ModifiedByNavigation { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<ViewApproval> ViewApprovals { get; set; } = new List<ViewApproval>();
}
