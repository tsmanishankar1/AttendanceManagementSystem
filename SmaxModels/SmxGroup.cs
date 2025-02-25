using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxGroup
{
    public decimal PkGrId { get; set; }

    public string? GrName { get; set; }

    public string? GrShortName { get; set; }

    public DateTime GrCreated { get; set; }

    public DateTime GrModified { get; set; }

    public string GrModifiedby { get; set; } = null!;
}
