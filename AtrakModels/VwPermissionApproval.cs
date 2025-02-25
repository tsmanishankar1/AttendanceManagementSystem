using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwPermissionApproval
{
    public string PermissionId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? FromTime { get; set; }

    public string? TimeTo { get; set; }

    public string? TotalHours { get; set; }

    public string Name { get; set; } = null!;

    public string? PermissionDate { get; set; }

    public string? PermissionOffReason { get; set; }

    public string? ContactNumber { get; set; }

    public int Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string Approval1StaffId { get; set; } = null!;

    public string Approval1StaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string? Approved1OnDate { get; set; }

    public string? Approved1OnTime { get; set; }

    public string? Comment { get; set; }

    public string ParentType { get; set; } = null!;

    public string Approval1Owner { get; set; } = null!;

    public string Iscancelledword { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public string? Approval2StatusId { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string? Approval2statusName { get; set; }

    public string? Approval2By { get; set; }

    public string? Approval2On { get; set; }

    public string? Approval2Owner { get; set; }

    public string? Approval2OwnerName { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }
}
