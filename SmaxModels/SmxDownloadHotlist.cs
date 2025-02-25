using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxDownloadHotlist
{
    public decimal HtId { get; set; }

    public string HtIpaddress { get; set; } = null!;

    public string HtUserId { get; set; } = null!;

    public decimal HtCardNumber { get; set; }
}
