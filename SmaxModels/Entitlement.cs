using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class Entitlement
{
    public int EnId { get; set; }

    public int FkUgGroupId { get; set; }

    public string FkScScreenName { get; set; } = null!;
}
