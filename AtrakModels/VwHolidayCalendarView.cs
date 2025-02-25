using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwHolidayCalendarView
{
    public int Hid { get; set; }

    public string? Leavetypeid { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? HolidayDateFrom { get; set; }

    public string? HolidayDateTo { get; set; }

    public int IsFixed { get; set; }

    public int? LeaveYear { get; set; }
}
