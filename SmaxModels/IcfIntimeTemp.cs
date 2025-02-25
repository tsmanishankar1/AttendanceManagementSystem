using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfIntimeTemp
{
    public DateTime? TrTime { get; set; }

    public string? TrIpaddress { get; set; }

    public DateOnly? TrDate1 { get; set; }

    public string? TrEmpid { get; set; }

    public int? TrId { get; set; }
}
