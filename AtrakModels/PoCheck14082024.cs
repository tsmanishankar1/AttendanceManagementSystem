using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PoCheck14082024
{
    public string StaffId { get; set; } = null!;

    public DateTime? WorkedDate { get; set; }

    public string? ExpAttStatus { get; set; }

    public string? Fhstatus { get; set; }

    public string? Shstatus { get; set; }

    public string? AttStatus { get; set; }
}
