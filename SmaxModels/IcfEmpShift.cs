using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfEmpShift
{
    public int IesId { get; set; }

    public string? IesEmpno { get; set; }

    public string? IesShift { get; set; }

    public DateOnly? IesDate { get; set; }

    public DateOnly? IesCreateddate { get; set; }

    public string? IesEmpsec { get; set; }
}
