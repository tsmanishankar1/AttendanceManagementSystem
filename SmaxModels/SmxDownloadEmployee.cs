using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxDownloadEmployee
{
    public decimal AdeId { get; set; }

    public decimal? AdeChCardno { get; set; }

    public string? AdeChCsnnumber { get; set; }

    public decimal? AdeAlId { get; set; }

    public string? AdeIpaddress { get; set; }

    public DateTime? AdeChValidto { get; set; }

    public byte[]? AdeChFinger1 { get; set; }

    public byte[]? AdeChFinger2 { get; set; }

    public decimal? AdeTzId { get; set; }

    public int? AdeMsId { get; set; }

    public bool? AdeDeleted { get; set; }

    public bool? AdeDwstatus { get; set; }

    public bool? AdeDwflag { get; set; }

    public bool? AdeIsbio { get; set; }

    public bool? AdeIscard { get; set; }

    public bool? AdeIscardBio { get; set; }

    public bool? AdeIspin { get; set; }

    public DateTime? AdeCreated { get; set; }

    public DateTime? AdeModified { get; set; }

    public string? AdeModifiedby { get; set; }
}
