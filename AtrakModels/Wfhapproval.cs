using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Wfhapproval
{
    public string ApplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? Applicantname { get; set; }

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? Odduration { get; set; }

    public string? OdfromDate { get; set; }

    public string? OdfromTime { get; set; }

    public string? OdtoDate { get; set; }

    public string? OdtoTime { get; set; }

    public string? Od { get; set; }

    public string? Odreason { get; set; }

    public int Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string? ApprovalStaffId { get; set; }

    public string? Approvalstaffname { get; set; }

    public string ApplicationApprovalId { get; set; } = null!;

    public string ApprovedOnDate { get; set; } = null!;

    public string? Comment { get; set; }

    public string ApprovalOwner { get; set; } = null!;

    public string? Approvalownername { get; set; }

    public string ParentType { get; set; } = null!;

    public string IsCancelled { get; set; } = null!;

    public int Approval2statusId { get; set; }

    public string? Approval2statusname { get; set; }

    public string? Approval2By { get; set; }

    public string Approval2on { get; set; } = null!;

    public string? Approval2Owner { get; set; }

    public string? Approval2ownername { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public string AppliedBy { get; set; } = null!;

    public string ApplicationType { get; set; } = null!;
}
