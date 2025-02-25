using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxShiftAssignmentDetail
{
    public decimal SftdId { get; set; }

    public decimal FkSftId { get; set; }

    public string FkEmpId { get; set; } = null!;

    public DateTime SftdDateTime { get; set; }

    public virtual SmxShiftDetail FkSft { get; set; } = null!;
}
