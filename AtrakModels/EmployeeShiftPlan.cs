using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeShiftPlan
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string ShiftId { get; set; } = null!;

    public int PatternId { get; set; }

    public int DayPatternId { get; set; }

    public string WeeklyOffId { get; set; } = null!;

    public bool IsGeneralShift { get; set; }

    public bool UseDayPattern { get; set; }

    public bool UseWeeklyOff { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public int LastUpdatedShiftId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsWeekPattern { get; set; }

    public DateTime StartDate { get; set; }

    public bool? InUse { get; set; }

    public int NoOfDaysShift { get; set; }

    public bool IsMonthlyPattern { get; set; }

    public string Reason { get; set; } = null!;

    public bool IsFlexiShift { get; set; }

    public bool IsAutoShift { get; set; }

    public bool IsManualShift { get; set; }
}
