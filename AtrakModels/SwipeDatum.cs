using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SwipeDatum
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public DateTime SwipeDate { get; set; }

    public DateTime SwipeTime { get; set; }

    public int InOut { get; set; }

    public bool IsManualPunch { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual Staff? Staff { get; set; }
}
