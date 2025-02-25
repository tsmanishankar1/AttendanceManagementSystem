using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttachDetachLog
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public bool IsAttached { get; set; }

    public string? ReportingManager { get; set; }

    public DateTime StateChangedOn { get; set; }
}
