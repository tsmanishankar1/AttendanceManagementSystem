using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxTimeZoneDetail
{
    public decimal TzdId { get; set; }

    public decimal TzdTzId { get; set; }

    public string TzdDays { get; set; } = null!;

    public string TzdStartTime { get; set; } = null!;

    public string TzdEndTime { get; set; } = null!;

    public DateTime? TzdSpecificDate { get; set; }

    public DateTime? TzdCreated { get; set; }

    public DateTime? TzdModified { get; set; }

    public string? TzdModifiedby { get; set; }

    public virtual SmxTimeZone TzdTz { get; set; } = null!;
}
