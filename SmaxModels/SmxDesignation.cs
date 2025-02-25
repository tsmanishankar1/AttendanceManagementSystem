using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxDesignation
{
    public int DnId { get; set; }

    public string DnName { get; set; } = null!;

    public string DnShortname { get; set; } = null!;

    public DateTime? DnCreated { get; set; }

    public DateTime? DnModified { get; set; }

    public string? DnModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
