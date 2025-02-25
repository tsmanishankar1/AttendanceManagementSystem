using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftExtensionHistory
{
    public string Id { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }

    public string StaffId { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string? Department { get; set; }

    public string? TxnDate { get; set; }

    public string? DurationOfHoursExtension { get; set; }

    public string? HoursBeforeShift { get; set; }

    public string? HoursAfterShift { get; set; }

    public string Iscancelled { get; set; } = null!;

    public string? CancelledBy { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string? CancelledDate { get; set; }

    public string Remarks { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string ApprovalOwner { get; set; } = null!;

    public int ApprovalStatusId { get; set; }

    public string? ApprovalStatus1 { get; set; }

    public string? Approval2Owner { get; set; }

    public int Approval2statusId { get; set; }

    public string? ApprovalStatus2 { get; set; }
}
