using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxAccessLevelReaderDetail
{
    public decimal AlId { get; set; }

    public string AlName { get; set; } = null!;

    public string DeName { get; set; } = null!;

    public decimal AldAlId { get; set; }

    public int DeId { get; set; }

    public string DeIpaddress { get; set; } = null!;

    public string AldReaderIpaddress { get; set; } = null!;
}
