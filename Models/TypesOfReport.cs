using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class TypesOfReport
{
    public int Id { get; set; }

    public string ReportName { get; set; } = null!;
}
