using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxBranch
{
    public int BrId { get; set; }

    public string BrName { get; set; } = null!;

    public DateTime? BrCreated { get; set; }

    public DateTime? BrModified { get; set; }

    public string BrModifiedby { get; set; } = null!;

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
