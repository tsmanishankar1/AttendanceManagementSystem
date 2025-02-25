using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxEventTask
{
    public decimal EtId { get; set; }

    public string EtDeviceIpaddress { get; set; } = null!;

    public int EtNodeId { get; set; }

    public string? EtIp3actR3Status { get; set; }

    public int? EtIp3actR3Time { get; set; }

    public bool? EtIp3actR3Message { get; set; }

    public int? EtIp3actR3MessageDuration { get; set; }

    public string? EtIp3nrmR3Status { get; set; }

    public int? EtIp3nrmR3Time { get; set; }

    public bool? EtIp3nrmR3Message { get; set; }

    public int? EtIp3nrmR3MessageDuration { get; set; }

    public string? EtIp4actR3Status { get; set; }

    public int? EtIp4actR3Time { get; set; }

    public bool? EtIp4actR3Message { get; set; }

    public int? EtIp4actR3MessageDuration { get; set; }

    public string? EtIp4nrmR3Status { get; set; }

    public int? EtIp4nrmR3Time { get; set; }

    public bool? EtIp4nrmR3Message { get; set; }

    public int? EtIp4nrmR3MessageDuration { get; set; }
}
