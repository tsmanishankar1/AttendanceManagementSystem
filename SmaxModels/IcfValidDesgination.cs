using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfValidDesgination
{
    public long? IvdId { get; set; }

    public string? IvdDesgcode { get; set; }

    public string? IvdDesgname { get; set; }

    public DateTime? IvdCreatedon { get; set; }
}
