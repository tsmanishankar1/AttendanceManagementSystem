using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxLeave
{
    public int LvId { get; set; }

    public string LvShortDesc { get; set; } = null!;

    public string? LvDescription { get; set; }

    public int? LvMaxDays { get; set; }

    public string? LvMaxAllowed { get; set; }

    public DateTime? LvCreated { get; set; }

    public DateTime? LvModified { get; set; }

    public string? LvModifiedby { get; set; }
}
