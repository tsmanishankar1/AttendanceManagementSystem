using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ElacApprovedAbsenceDate
{
    public string StaffId { get; set; } = null!;

    public string? LeaveTypeId { get; set; }

    public string? LeaveShortName { get; set; }

    public int IsAccountable { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? LeaveApplicationReason { get; set; }

    public int ApprovalStatusId { get; set; }

    public string? ApprovalStaffName { get; set; }

    public string ApprovalStaffId { get; set; } = null!;

    public int LeaveStartDurationId { get; set; }

    public int LeaveEndDurationId { get; set; }

    public bool IsCancelled { get; set; }

    public bool IsRejected { get; set; }

    public string ParentType { get; set; } = null!;
}
