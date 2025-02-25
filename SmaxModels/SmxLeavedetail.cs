using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxLeavedetail
{
    public decimal LdId { get; set; }

    public int? FkLvId { get; set; }

    public DateTime? LdDateLdetails { get; set; }

    public decimal? LdDuration { get; set; }

    public string? LdChId { get; set; }

    public decimal? LdUnit { get; set; }

    public DateTime? LdCreated { get; set; }

    public DateTime? LdModified { get; set; }

    public string? LdModifiedby { get; set; }
}
