using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxAreasdownload
{
    public string ArName { get; set; } = null!;

    public string? ArType { get; set; }

    public decimal? ArApb { get; set; }

    public string? ArIpaddress { get; set; }

    public decimal? ArApbnumber { get; set; }

    public decimal? DeDotz { get; set; }
}
