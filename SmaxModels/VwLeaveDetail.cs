using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwLeaveDetail
{
    public string? EmpId { get; set; }

    public string LeaveDuration { get; set; } = null!;

    public string? LeaveDate { get; set; }

    public string EmpName { get; set; } = null!;

    public string? LeaveDescription { get; set; }
}
