using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Leavebalance
{
    public string StaffId { get; set; } = null!;

    public string? LeaveGroupId { get; set; }

    public string? LeaveTypeId { get; set; }

    public string LeaveName { get; set; } = null!;

    public bool IsActive { get; set; }

    public decimal? Leavebalance1 { get; set; }

    public string? AvailableBalance { get; set; }
}
