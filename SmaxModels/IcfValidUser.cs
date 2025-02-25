using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfValidUser
{
    public long IvuId { get; set; }

    public string IvuUsername { get; set; } = null!;

    public string? IvuPassword { get; set; }

    public string? IvuUsertype { get; set; }

    public DateTime? IvuCreatedOn { get; set; }

    public string? IvuCreatedBy { get; set; }
}
