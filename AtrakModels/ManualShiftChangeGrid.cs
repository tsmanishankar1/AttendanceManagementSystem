using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ManualShiftChangeGrid
{
    public int Id { get; set; }

    public string? StaffId { get; set; }

    public string? ShiftId { get; set; }

    public DateTime? TxnDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }
}
