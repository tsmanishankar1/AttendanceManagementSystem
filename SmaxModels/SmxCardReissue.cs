using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxCardReissue
{
    public decimal CrId { get; set; }

    public string CrChEmpId { get; set; } = null!;

    public decimal CrChCardNo { get; set; }

    public DateTime CrCreated { get; set; }
}
