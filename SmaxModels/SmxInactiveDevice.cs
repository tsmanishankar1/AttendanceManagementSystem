using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxInactiveDevice
{
    public decimal DsId { get; set; }

    public string DsIpadress { get; set; } = null!;

    public string DsDeviceName { get; set; } = null!;

    public string? DsNotes { get; set; }

    public DateTime DsCreated { get; set; }

    public DateTime DsModified { get; set; }

    public string DsModifiedby { get; set; } = null!;
}
