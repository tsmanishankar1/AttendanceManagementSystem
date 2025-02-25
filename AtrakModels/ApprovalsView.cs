using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ApprovalsView
{
    public string LeaveApplicationId { get; set; } = null!;

    public string? StaffId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int LeaveDurationId { get; set; }

    public string? LeaveStartDate { get; set; }

    public string? LeaveEndDate { get; set; }

    public string? ApplicationDate { get; set; }

    public string LeaveReason { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public string Cancelled { get; set; } = null!;

    public int ApprovalId { get; set; }

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string LeaveName { get; set; } = null!;

    public bool IsPermission { get; set; }

    public string Permission { get; set; } = null!;

    public int StaffStatusId { get; set; }

    public string? StaffName { get; set; }

    public string LeaveDurationName { get; set; } = null!;

    public string StaffStatusName { get; set; } = null!;
}
