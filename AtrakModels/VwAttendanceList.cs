using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwAttendanceList
{
    public string? StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? ShiftId { get; set; }

    public string? ShiftName { get; set; }

    public string? LeaveName { get; set; }

    public string? CompanyName { get; set; }

    public string? DepartmentName { get; set; }

    public string? GradeName { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? ActualOutDate { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ActualWorkedHours { get; set; }

    public string AttendanceStatus { get; set; } = null!;
}
