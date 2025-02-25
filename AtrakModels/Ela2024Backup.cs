using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Ela2024Backup
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int TransactionFlag { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal LeaveCount { get; set; }

    public string Narration { get; set; } = null!;

    public string? RefId { get; set; }

    public DateTime? FinancialYearStart { get; set; }

    public DateTime? FinancialYearEnd { get; set; }

    public int LeaveCreditDebitReasonId { get; set; }

    public string? TransctionBy { get; set; }

    public bool IsSystemAction { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public bool IsManuallyExtended { get; set; }

    public decimal ExtensionPeriod { get; set; }

    public DateTime? WorkedDate { get; set; }

    public bool IsProcessed { get; set; }
}
