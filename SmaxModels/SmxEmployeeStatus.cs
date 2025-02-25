using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxEmployeeStatus
{
    public int EsId { get; set; }

    public string EsName { get; set; } = null!;

    public string EsShortname { get; set; } = null!;

    public DateTime? EsCreated { get; set; }

    public DateTime? EsModified { get; set; }

    public string? EsModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
