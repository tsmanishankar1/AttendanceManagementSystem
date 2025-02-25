using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwLeaveApplicationList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? StartDate { get; set; }

    public string? LeaveStartDurationId { get; set; }

    public string StartDuration { get; set; } = null!;

    public string? EndDate { get; set; }

    public string? LeaveEndDurationId { get; set; }

    public string EndDuration { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveName { get; set; } = null!;

    public string? ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public int Reasonid { get; set; }

    public string Reason { get; set; } = null!;

    public decimal Totaldays { get; set; }

    public bool Iscancelled { get; set; }
}
