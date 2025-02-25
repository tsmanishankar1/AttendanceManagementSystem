using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class OtbookingDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string Otdate { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Ottype { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }
}
