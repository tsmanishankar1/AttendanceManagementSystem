using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwManualPunchList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? InDate { get; set; }

    public string? InTime { get; set; }

    public string? OutDate { get; set; }

    public string? OutTime { get; set; }

    public string Reason { get; set; } = null!;

    public string? StatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? PunchType { get; set; }
}
