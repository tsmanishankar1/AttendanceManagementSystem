using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftChangeApproval
{
    public string ApplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? StaffName { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string NewShiftName { get; set; } = null!;

    public string TotalHours { get; set; } = null!;

    public string? Remarks { get; set; }

    public int Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string Approval1Owner { get; set; } = null!;

    public string? Approval1OwnerName { get; set; }

    public string ApplicationApprovalId { get; set; } = null!;

    public string Approved1On { get; set; } = null!;

    public string? Comment { get; set; }

    public string ParentType { get; set; } = null!;

    public string Iscancelled { get; set; } = null!;

    public int Approval2statusId { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string? Approval2statusName { get; set; }

    public string? Approval2By { get; set; }

    public string Approval2On { get; set; } = null!;

    public string? Approval2Owner { get; set; }

    public string? Approval2OwnerName { get; set; }
}
