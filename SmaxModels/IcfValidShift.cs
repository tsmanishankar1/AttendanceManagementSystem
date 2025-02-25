using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfValidShift
{
    public int IvsId { get; set; }

    public string? IvsScode { get; set; }

    public string? IvsStart { get; set; }

    public string? IvsEnd { get; set; }

    public string? IvsBreakStart { get; set; }

    public string? IvsBreakEnd { get; set; }

    public string? IvsSatStart { get; set; }

    public string? IvsSatEnd { get; set; }

    public string? IvsSind { get; set; }

    public DateTime? IvsCreateddate { get; set; }

    public string? IvsCreatedby { get; set; }
}
