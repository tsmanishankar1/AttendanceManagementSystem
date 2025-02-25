using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Vwabsencedate
{
    public string StaffId { get; set; } = null!;

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public string Iscancelled { get; set; } = null!;
}
