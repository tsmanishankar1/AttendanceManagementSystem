using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwLaterOffApproval
{
    public string CoffId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? LaterOffReqDate { get; set; }

    public string? LaterOffAvailDate { get; set; }

    public string LaterOffReason { get; set; } = null!;

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string ApprovalStaffId { get; set; } = null!;

    public string ApprovalStaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string Comment { get; set; } = null!;

    public string ApprovalOwner { get; set; } = null!;

    public string Iscancelled { get; set; } = null!;
}
