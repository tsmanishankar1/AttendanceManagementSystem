using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftsImportDump
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? Shift { get; set; }

    public DateTime? ShiftFromDate { get; set; }

    public DateTime? ShiftToDate { get; set; }

    public bool? IsProcessed { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ExcelFileName { get; set; }
}
