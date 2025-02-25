using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Smaxadvmaster
{
    public string ChId { get; set; } = null!;

    public string? Department { get; set; }

    public string? Grade { get; set; }

    public string? Unit { get; set; }

    public string? Designation { get; set; }

    public string? Trade { get; set; }
}
