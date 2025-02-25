using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxShiftDetail
{
    public decimal SftdId { get; set; }

    public string SftdName { get; set; } = null!;

    public string SftdStartTime { get; set; } = null!;

    public string SftdEndTime { get; set; } = null!;

    public decimal? SftdHoursId { get; set; }

    public DateTime? SftdCreated { get; set; }

    public DateTime? SftdModified { get; set; }

    public string? SftdModifiedby { get; set; }

    public virtual ICollection<SmxShiftAssignmentDetail> SmxShiftAssignmentDetails { get; set; } = new List<SmxShiftAssignmentDetail>();
}
