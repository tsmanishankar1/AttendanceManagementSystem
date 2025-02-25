using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxMessage
{
    public int MsId { get; set; }

    public string MsName { get; set; } = null!;

    public string MsLine1 { get; set; } = null!;

    public string? MsLine2 { get; set; }

    public DateTime? MsCreated { get; set; }

    public DateTime? MsModified { get; set; }

    public string? MsModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
