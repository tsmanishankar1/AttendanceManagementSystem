using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class Passwordhistory
{
    public int PhId { get; set; }

    public string? PhUsPassword { get; set; }

    public int? FkUsId { get; set; }

    public DateTime PhUsCreated { get; set; }

    public DateTime? PhUsModified { get; set; }

    public virtual User? FkUs { get; set; }
}
