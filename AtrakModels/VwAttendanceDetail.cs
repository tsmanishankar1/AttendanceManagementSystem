using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwAttendanceDetail
{
    public string? StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? BranchName { get; set; }

    public string? DeptName { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ActualWorkedHours { get; set; }

    public DateTime? Othours { get; set; }
}
