using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftExtensionDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string ExtensionType { get; set; } = null!;

    public DateTime ShiftExtensionDate { get; set; }

    public DateTime? BeforeShiftHours { get; set; }

    public DateTime? AfterShiftHours { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }
}
