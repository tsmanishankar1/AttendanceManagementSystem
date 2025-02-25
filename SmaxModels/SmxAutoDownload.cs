using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxAutoDownload
{
    public decimal AdId { get; set; }

    public decimal? AdChCardno { get; set; }

    public string? AdChCsnnumber { get; set; }

    public decimal? AdAlId { get; set; }

    public string? AdIpaddress { get; set; }

    public bool? AdDeleted { get; set; }

    public bool? AdDwstatus { get; set; }

    public bool? AdDwflag { get; set; }

    public bool? AdIsbio { get; set; }

    public bool? AdIscard { get; set; }

    public bool? AdIscardBio { get; set; }

    public bool? AdIspin { get; set; }

    public DateTime? AdCreated { get; set; }

    public DateTime? AdModified { get; set; }

    public string? AdModifiedby { get; set; }
}
