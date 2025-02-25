using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftsImportDatum
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string ShiftId { get; set; } = null!;

    public DateTime ShiftFromDate { get; set; }

    public DateTime ShiftToDate { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ProcessedOn { get; set; }
}
