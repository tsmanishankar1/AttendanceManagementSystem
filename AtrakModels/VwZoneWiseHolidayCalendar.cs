using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwZoneWiseHolidayCalendar
{
    public string? LeaveTypeId { get; set; }

    public string LeaveTypeName { get; set; } = null!;

    public DateTime HolidayDateFrom { get; set; }

    public DateTime HolidayDateTo { get; set; }

    public string? HolidayGroupId { get; set; }

    public string HolidayGroupName { get; set; } = null!;

    public int HolidayZoneId { get; set; }

    public string HolidayZoneName { get; set; } = null!;

    public string LeaveTypeShortName { get; set; } = null!;
}
