using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Setting
{
    public int Id { get; set; }

    public string Parameter { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Value { get; set; }

    public string? DefaultValue { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;
}
