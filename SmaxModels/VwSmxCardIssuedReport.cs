using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxCardIssuedReport
{
    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Location { get; set; } = null!;
}
