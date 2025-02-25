using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxDepartment
{
    public int DpId { get; set; }

    public string DpName { get; set; } = null!;

    public string DpShortname { get; set; } = null!;

    public DateTime? DpCreated { get; set; }

    public DateTime? DpModified { get; set; }

    public string? DpModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
