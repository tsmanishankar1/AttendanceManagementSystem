using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RawPunchDetail
{
    public string? TrChid { get; set; }

    public DateTime? TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public string? DeReadertype { get; set; }
}
