using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ManualShiftConfiguration
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string ShiftId { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
