using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveApplicationDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string LeaveType { get; set; } = null!;

    public string StartDuration { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public string EndDuration { get; set; } = null!;

    public DateTime EndDate { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }
}
