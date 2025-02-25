using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Association
{
    public int Id { get; set; }

    public string Combination { get; set; } = null!;

    public string ParentId { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public int Priority { get; set; }

    public string WorkingDayPattern { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
