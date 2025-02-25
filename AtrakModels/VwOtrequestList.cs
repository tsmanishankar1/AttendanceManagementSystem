using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwOtrequestList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Otdate { get; set; }

    public string Ottime { get; set; } = null!;

    public string? Otduration { get; set; }

    public string Otreason { get; set; } = null!;

    public int StatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
