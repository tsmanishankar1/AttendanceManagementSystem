using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ErrorLog
{
    public long Id { get; set; }

    public string AppName { get; set; } = null!;

    public string ModuleName { get; set; } = null!;

    public string FunctionName { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? StaffId { get; set; }

    public DateTime? TxnDate { get; set; }

    public int? LineNumber { get; set; }
}
