using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PermissionDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime PermissionDate { get; set; }

    public DateTime FromTime { get; set; }

    public DateTime ToTime { get; set; }

    public string PermissionType { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }
}
