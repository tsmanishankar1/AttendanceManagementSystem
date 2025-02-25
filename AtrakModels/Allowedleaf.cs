using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Allowedleaf
{
    public int LeaveGroupTxnId { get; set; }

    public string? LeaveGroupId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int LeaveCount { get; set; }

    public int MaxSeqLeaves { get; set; }

    public bool LeaveGroupTxnIsActive { get; set; }

    public string LeaveTypeName { get; set; } = null!;

    public string LeaveTypeShortName { get; set; } = null!;

    public bool IsAccountable { get; set; }

    public bool IsEncashable { get; set; }

    public bool IsPaidLeave { get; set; }

    public bool CarryForward { get; set; }

    public bool IsCommon { get; set; }

    public bool IsPermission { get; set; }

    public bool LeaveTypeIsActive { get; set; }

    public string LeaveGroupName { get; set; } = null!;

    public bool LeaveGroupIsActive { get; set; }

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public decimal? Leavebalance { get; set; }
}
