using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwOdrequestList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Odduration { get; set; } = null!;

    public string? Oddate { get; set; }

    public string? From { get; set; }

    public string? To { get; set; }

    public string Odreason { get; set; } = null!;

    public string? StatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool IsCancelled { get; set; }
}
