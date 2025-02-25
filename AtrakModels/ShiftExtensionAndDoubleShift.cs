using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftExtensionAndDoubleShift
{
    public int Id { get; set; }

    public string? StaffId { get; set; }

    public DateTime TxnDate { get; set; }

    public string? ShiftExtensionType { get; set; }

    public string? DurationOfHoursExtension { get; set; }

    public int NoOfHoursBeforeShift { get; set; }

    public int NoOfHoursAfterShift { get; set; }

    public string? Shift1 { get; set; }

    public string? Shift2 { get; set; }

    public string? Shift3 { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
