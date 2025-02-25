using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxDownloadEmployee
{
    public string ChEmpId { get; set; } = null!;

    public string ChFname { get; set; } = null!;

    public string? ChCsnnumber { get; set; }

    public string? AdeIpAddress { get; set; }

    public bool? AdeDwflag { get; set; }

    public string DeName { get; set; } = null!;

    public bool? AdeIsbio { get; set; }

    public DateTime? AdeCreated { get; set; }
}
