using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Shiftwisemail
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? AbsentCountLastUpdateddate { get; set; }

    public DateTime? PresentCountLastUpdateddate { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
