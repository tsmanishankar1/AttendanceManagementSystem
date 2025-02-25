using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwOtapproval
{
    public string OtapplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Otdate { get; set; }

    public string Ottime { get; set; } = null!;

    public string? Otduration { get; set; }

    public string Otreason { get; set; } = null!;

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string ApprovalStaffId { get; set; } = null!;

    public string ApprovalStaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string? Comment { get; set; }

    public string ApprovalOwner { get; set; } = null!;

    public string Iscancelled { get; set; } = null!;
}
