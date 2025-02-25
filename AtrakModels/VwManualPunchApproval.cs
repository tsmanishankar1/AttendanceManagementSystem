using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwManualPunchApproval
{
    public string ManualPunchId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? PunchType { get; set; }

    public string? InDate { get; set; }

    public string? InTime { get; set; }

    public string? OutDate { get; set; }

    public string? OutTime { get; set; }

    public string? ManualPunchReason { get; set; }

    public string FirstName { get; set; } = null!;

    public string? Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string Approval1StaffId { get; set; } = null!;

    public string Approval1StaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string? Comment { get; set; }

    public string Approval1Owner { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public string? Approval2statusId { get; set; }

    public string? Approval2OnDate { get; set; }

    public string? Approval2Owner { get; set; }

    public string? Approval2OwnerName { get; set; }

    public bool IsCancelled { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string? Approval2statusName { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }
}
