using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxGroupDetail
{
    public decimal GdId { get; set; }

    public decimal FkGdId { get; set; }

    public int FkGdUnit { get; set; }

    public string GdChid { get; set; } = null!;

    public decimal? FkSftId { get; set; }

    public DateTime GdCreated { get; set; }

    public DateTime GdModified { get; set; }

    public string GdModifiedby { get; set; } = null!;

    public virtual SmxGroup FkGd { get; set; } = null!;

    public virtual SmxShiftDetail? FkSft { get; set; }
}
