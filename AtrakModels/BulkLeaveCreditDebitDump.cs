using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class BulkLeaveCreditDebitDump
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? LeaveType { get; set; }

    public string? TransactionType { get; set; }

    public decimal? LeaveCount { get; set; }

    public string? LeaveCreditDebitReason { get; set; }

    public string? Month { get; set; }

    public int? Year { get; set; }

    public bool? IsError { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? ExcelFileName { get; set; }
}
