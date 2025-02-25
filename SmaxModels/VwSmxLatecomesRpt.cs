using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxLatecomesRpt
{
    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? CardNumber { get; set; }

    public DateOnly? TrDate { get; set; }

    public string? Date { get; set; }

    public string? Time { get; set; }
}
