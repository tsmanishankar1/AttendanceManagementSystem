using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwRhapproval
{
    public string RhapplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? Applicantname { get; set; }

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? RhfromDate { get; set; }

    public string? RhtoDate { get; set; }

    public string Rhreason { get; set; } = null!;

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string ApprovalStaffId { get; set; } = null!;

    public string? Approvalstaffname { get; set; }

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string Comment { get; set; } = null!;

    public string ApprovalOwner { get; set; } = null!;

    public string? Approvalownername { get; set; }

    public string Iscancelled { get; set; } = null!;
}
