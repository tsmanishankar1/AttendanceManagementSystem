using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class StaffFamily
{
    public string Id { get; set; } = null!;

    public string? StaffId { get; set; }

    public int RelatedAs { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }

    public virtual RelationType RelatedAsNavigation { get; set; } = null!;

    public virtual Staff? Staff { get; set; }
}
