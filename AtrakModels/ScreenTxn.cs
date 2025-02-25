using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ScreenTxn
{
    public int Id { get; set; }

    public string LocationId { get; set; } = null!;

    public int RoleId { get; set; }

    public int ScreenId { get; set; }

    public int IsMainMenu { get; set; }

    public int ParentId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public virtual Screen1 Screen { get; set; } = null!;
}
