using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class User
{
    public int UsId { get; set; }

    public string UsUser { get; set; } = null!;

    public string? UsLogin { get; set; }

    public string? UsPassword { get; set; }

    public int FkUgGroupId { get; set; }

    public DateTime UsCreated { get; set; }

    public DateTime UsModified { get; set; }
}
