using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxCategory
{
    public int CtId { get; set; }

    public string CtName { get; set; } = null!;

    public string CtShortname { get; set; } = null!;

    public DateTime? CtCreated { get; set; }

    public DateTime? CtModified { get; set; }

    public string? CtModifiedby { get; set; }

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
