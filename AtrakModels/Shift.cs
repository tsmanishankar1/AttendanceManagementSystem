using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Shift
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime? GraceLateBy { get; set; }

    public DateTime? GraceEarlyBy { get; set; }

    public DateTime? BreakStartTime { get; set; }

    public DateTime? BreakEndTime { get; set; }

    public decimal MinDayHours { get; set; }

    public decimal MinWeekHours { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? LocationId { get; set; }

    public int? HolidayZoneId { get; set; }

    public string? HolidayGroupId { get; set; }
}
