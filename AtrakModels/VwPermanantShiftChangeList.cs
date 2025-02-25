using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwPermanantShiftChangeList
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string? Department { get; set; }

    public string ShiftName { get; set; } = null!;

    public string PatternName { get; set; } = null!;

    public string WorkingDayPattern { get; set; } = null!;

    public string? WithEffectFrom { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool IsGeneralShift { get; set; }
}
