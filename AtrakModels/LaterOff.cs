using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LaterOff
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public DateTime LaterOffReqDate { get; set; }

    public DateTime LaterOffAvailDate { get; set; }

    public string Reason { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
