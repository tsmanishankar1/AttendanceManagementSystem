using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxCardAccesslevelEmployee
{
    public string? ChCardNo { get; set; }

    public string ChEmpId { get; set; } = null!;

    public string ChFname { get; set; } = null!;

    public string DeName { get; set; } = null!;

    public string AlName { get; set; } = null!;

    public string LnName { get; set; } = null!;

    public decimal? CalAlId { get; set; }
}
