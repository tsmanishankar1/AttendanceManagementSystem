using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftExtensionApproval
{
    public string ShiftExtensionId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? ExtensionDate { get; set; }

    public string? DurationOfHoursExtension { get; set; }

    public string? HoursBeforeShift { get; set; }

    public string? HoursAfterShift { get; set; }

    public string? ShiftExtensionReason { get; set; }

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
}
