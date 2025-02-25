using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class OnDutyDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string Odtype { get; set; } = null!;

    public string FromDuration { get; set; } = null!;

    public DateTime From { get; set; }

    public string ToDuration { get; set; } = null!;

    public DateTime To { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }
}
