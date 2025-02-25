using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwShiftChangeApproval
{
    public string ShiftChangeId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public string NewShiftId { get; set; } = null!;

    public string NewShiftName { get; set; } = null!;

    public string? ShiftChangeReason { get; set; }

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string ApprovalStaffId { get; set; } = null!;

    public string ApprovalStaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string Approval2StaffId { get; set; } = null!;

    public string Approval2StaffName { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string? Comment { get; set; }

    public string ApprovalOwner { get; set; } = null!;

    public string? Approval2Owner { get; set; }

    public int Approval2StatusId { get; set; }

    public bool IsCancelled { get; set; }
}
