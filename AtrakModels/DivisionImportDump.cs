using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class DivisionImportDump
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ExcelFileName { get; set; }

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool? IsProcessed { get; set; }
}
