using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxArea
{
    public decimal ArId { get; set; }

    public string ArName { get; set; } = null!;

    public int? ArNodeid { get; set; }

    public string? ArType { get; set; }

    public decimal? ArApb { get; set; }

    public int? ArLnId { get; set; }

    public string? ArIpaddress { get; set; }

    public decimal? ArApbnumber { get; set; }

    public string? ArStatus { get; set; }

    public bool? ArDeleted { get; set; }

    public virtual SmxLocation? ArLn { get; set; }
}
