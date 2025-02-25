using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VleadDec23AttStatusChange
{
    public string? StaffId { get; set; }

    public string? AttendanceDate { get; set; }

    public string? AttendanceStatus { get; set; }

    public string? Fhstatus { get; set; }

    public string? Shstatus { get; set; }

    public string? DayAccount { get; set; }

    public string? AbsentCount { get; set; }
}
