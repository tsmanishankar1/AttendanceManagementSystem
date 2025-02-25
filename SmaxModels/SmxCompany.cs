using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxCompany
{
    public int CgId { get; set; }

    public string CgName { get; set; } = null!;

    public string CgShortname { get; set; } = null!;

    public string CgHead { get; set; } = null!;

    public string? CgAddress { get; set; }

    public string? CgCity { get; set; }

    public string? CgPhone { get; set; }

    public string? CgFax { get; set; }

    public string? CgEmail { get; set; }

    public DateTime CgCreated { get; set; }

    public DateTime CgModified { get; set; }

    public string CgModifiedby { get; set; } = null!;

    public virtual ICollection<SmxCardHolder> SmxCardHolders { get; set; } = new List<SmxCardHolder>();
}
