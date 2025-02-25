using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeLeaveAccountCalendar
{
    public string? StaffId { get; set; }

    public string? LeaveTypeId { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public int? TransactionFlag { get; set; }

    public decimal? LeaveCount { get; set; }

    public DateOnly? FystartYear { get; set; }

    public DateOnly? FyendYear { get; set; }

    public int? LeaveCreditDebitReasonId { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public string? RefId { get; set; }

    public long? ElaId { get; set; }
}
