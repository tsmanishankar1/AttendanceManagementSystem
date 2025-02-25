using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxLocation
{
    public int LnId { get; set; }

    public string LnName { get; set; } = null!;

    public string? LnAddress { get; set; }

    public string LnShortname { get; set; } = null!;

    public DateTime? LnCreated { get; set; }

    public DateTime? LnModified { get; set; }

    public string? LnModifiedby { get; set; }

    public virtual ICollection<SmxAccessLevelDetail> SmxAccessLevelDetails { get; set; } = new List<SmxAccessLevelDetail>();

    public virtual ICollection<SmxArea> SmxAreas { get; set; } = new List<SmxArea>();

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();

    public virtual ICollection<SmxDevice> SmxDevices { get; set; } = new List<SmxDevice>();
}
