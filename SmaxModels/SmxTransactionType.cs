using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxTransactionType
{
    public int TtId { get; set; }

    public int TtCode { get; set; }

    public string TtDescription { get; set; } = null!;
}
