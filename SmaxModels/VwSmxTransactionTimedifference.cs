using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxTransactionTimedifference
{
    public string? TrCsnnumber { get; set; }

    public string? TrIpaddress { get; set; }

    public DateTime? TrDate { get; set; }

    public DateTime? TrCreated { get; set; }

    public DateTime? Timedifference { get; set; }
}
