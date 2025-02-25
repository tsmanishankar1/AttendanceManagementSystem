using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class Emp
{
    public string Empcode { get; set; } = null!;

    public DateOnly Logdate { get; set; }

    public TimeOnly Logtime { get; set; }

    public string Workcode { get; set; } = null!;
}
