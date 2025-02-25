using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxAccessLevelDetail
{
    public decimal AldId { get; set; }

    public decimal AldAlId { get; set; }

    public int AldNodeid { get; set; }

    public decimal AldTzId { get; set; }

    public string AldReaderIpaddress { get; set; } = null!;

    public int AldLnId { get; set; }

    public DateTime? AldCreated { get; set; }

    public DateTime? AldModified { get; set; }

    public string? AldModifiedby { get; set; }

    public virtual SmxAccessLevel AldAl { get; set; } = null!;

    public virtual SmxLocation AldLn { get; set; } = null!;

    public virtual SmxTimeZone AldTz { get; set; } = null!;
}
