using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PermissionType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
}
