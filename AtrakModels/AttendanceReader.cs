using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceReader
{
    public int Id { get; set; }

    public string IpAddress { get; set; } = null!;

    public bool? IsCanteenReader { get; set; }
}
