using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeDeletionDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime ResignationDate { get; set; }

    public DateTime RelievingDate { get; set; }

    public string? Status { get; set; }

    public bool? IsProcessed { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ExcelFileName { get; set; }
}
