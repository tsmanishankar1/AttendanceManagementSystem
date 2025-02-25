using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PermissionApp
{
    public string StaffId { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public string? Fhstatus { get; set; }

    public string? Shstatus { get; set; }

    public string? AttendanceStatus { get; set; }
}
