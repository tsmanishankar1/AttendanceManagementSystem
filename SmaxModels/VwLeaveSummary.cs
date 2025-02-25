using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwLeaveSummary
{
    public string ChEmpId { get; set; } = null!;

    public string ChFname { get; set; } = null!;

    public string LvShortDesc { get; set; } = null!;

    public int? MaxDays { get; set; }

    public decimal? LeaveTaken { get; set; }
}
