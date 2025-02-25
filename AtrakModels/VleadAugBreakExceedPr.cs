using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VleadAugBreakExceedPr
{
    public string? Staffid { get; set; }

    public string? ShiftName { get; set; }

    public string? ShiftInDate { get; set; }

    public string? BreakHours { get; set; }

    public string? IsBreakExceed { get; set; }

    public string? ProductiveHours { get; set; }

    public string? AttendanceStatus { get; set; }

    public string? PermissionId { get; set; }

    public bool? IsManualStatus { get; set; }

    public string? ExpectedWorkingHours { get; set; }

    public string? Comments { get; set; }
}
