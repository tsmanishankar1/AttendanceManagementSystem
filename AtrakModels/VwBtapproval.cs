using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwBtapproval
{
    public string OdapplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? Applicantname { get; set; }

    public string? LeaveTypeId { get; set; }

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? Oduration { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public string? Total { get; set; }

    public string? Reason { get; set; }

    public int Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string? ApprovalStaffId { get; set; }

    public string? Approvalstaffname { get; set; }

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string? Comment { get; set; }

    public string ApprovalOwner { get; set; } = null!;

    public string? Approvalownername { get; set; }

    public string ParentType { get; set; } = null!;

    public string IsCancelled { get; set; } = null!;

    public int Approval2statusId { get; set; }

    public string? Approval2statusname { get; set; }

    public string? Approval2By { get; set; }

    public string? Approval2on { get; set; }

    public string? Approval2Owner { get; set; }

    public string? Approval2ownername { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public string AppliedBy { get; set; } = null!;

    public string ApplicationType { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }
}
