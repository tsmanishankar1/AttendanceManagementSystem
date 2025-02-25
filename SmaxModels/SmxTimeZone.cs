using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxTimeZone
{
    public decimal TzId { get; set; }

    public string? TzName { get; set; }

    public bool? TzUpdateStatus { get; set; }

    public DateTime? TzCreated { get; set; }

    public DateTime? TzModified { get; set; }

    public string? TzModifiedby { get; set; }

    public virtual ICollection<SmxAccessLevelDetail> SmxAccessLevelDetails { get; set; } = new List<SmxAccessLevelDetail>();

    public virtual ICollection<SmxTimeZoneDetail> SmxTimeZoneDetails { get; set; } = new List<SmxTimeZoneDetail>();
}
