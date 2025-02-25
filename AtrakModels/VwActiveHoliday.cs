using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwActiveHoliday
{
    public long HolidayGroupId { get; set; }

    public int LeaveYear { get; set; }

    public int HolidayId { get; set; }

    public string? LeaveTypeId { get; set; }

    public string HolidayName { get; set; } = null!;

    public DateTime HolidayDateFrom { get; set; }

    public DateTime HolidayDateTo { get; set; }

    public string LeaveTypeShortName { get; set; } = null!;

    public bool IsHoliday { get; set; }
}
