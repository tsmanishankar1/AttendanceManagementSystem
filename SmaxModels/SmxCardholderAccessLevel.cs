using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxCardholderAccessLevel
{
    public decimal CalId { get; set; }

    public decimal? CalChCardno { get; set; }

    public string? CalChCsnnumber { get; set; }

    public decimal? CalAlId { get; set; }

    public bool? CalDeleted { get; set; }

    public bool? CalDwstatus { get; set; }

    public DateTime? CalCreated { get; set; }

    public DateTime? CalModified { get; set; }

    public string? CalModifiedby { get; set; }

    public virtual SmxAccessLevel? CalAl { get; set; }
}
