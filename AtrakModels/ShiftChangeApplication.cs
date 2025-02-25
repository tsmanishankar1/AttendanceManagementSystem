using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftChangeApplication
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string NewShiftId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
