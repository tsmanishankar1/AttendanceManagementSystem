using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwCoffAvailedHistory
{
    public string CoffId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? Name { get; set; }

    public string LeaveTypeId { get; set; } = null!;

    public string? TotalDays { get; set; }

    public string LeaveShortName { get; set; } = null!;

    public string? WorkedDate { get; set; }

    public string? AvailDate { get; set; }

    public string? CoffReason { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string Iscancelled { get; set; } = null!;
}
