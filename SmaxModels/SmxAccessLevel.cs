using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxAccessLevel
{
    public decimal AlId { get; set; }

    public string AlName { get; set; } = null!;

    public DateTime? AlCreated { get; set; }

    public DateTime? AlModified { get; set; }

    public string? AlModifiedby { get; set; }

    public virtual ICollection<SmxAccessLevelDetail> SmxAccessLevelDetails { get; set; } = new List<SmxAccessLevelDetail>();

    public virtual ICollection<SmxCardholderAccessLevel> SmxCardholderAccessLevels { get; set; } = new List<SmxCardholderAccessLevel>();
}
