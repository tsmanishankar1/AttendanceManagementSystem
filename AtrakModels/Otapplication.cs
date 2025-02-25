using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Otapplication
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public DateTime Otdate { get; set; }

    public string Ottime { get; set; } = null!;

    public DateTime Otduration { get; set; }

    public string Otreason { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime InTime { get; set; }

    public DateTime OutTime { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
