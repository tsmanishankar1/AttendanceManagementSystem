using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxUnit
{
    public int UtId { get; set; }

    public string UtName { get; set; } = null!;

    public string UtDescription { get; set; } = null!;

    public DateTime? UtCreated { get; set; }

    public DateTime? UtModified { get; set; }

    public string? UtModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
