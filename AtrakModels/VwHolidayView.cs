using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwHolidayView
{
    public int Id { get; set; }

    public string? LeaveTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? FixedHolidayDateFrom { get; set; }

    public DateTime? FixedHolidayDateTo { get; set; }

    public int? IsFixed { get; set; }
}
