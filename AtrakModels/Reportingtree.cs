using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Reportingtree
{
    public string Reporteeid { get; set; } = null!;

    public string Reportingmgrid { get; set; } = null!;

    public string Repmgrfirstname { get; set; } = null!;

    public string? Repmgrmiddlename { get; set; }

    public string Repmgrlastname { get; set; } = null!;
}
