using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SubordinateTree
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string ReportingStaffId { get; set; } = null!;

    public string Signature { get; set; } = null!;
}
