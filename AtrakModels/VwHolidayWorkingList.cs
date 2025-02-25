using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwHolidayWorkingList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Department { get; set; }

    public string? TransactionDate { get; set; }

    public string? InTime { get; set; }

    public string? OutTime { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string IsCancelled { get; set; } = null!;

    public string ApproverStatus1 { get; set; } = null!;

    public string ApproverStatus2 { get; set; } = null!;

    public string ApprovalOwner { get; set; } = null!;

    public string? Approval2Owner { get; set; }
}
