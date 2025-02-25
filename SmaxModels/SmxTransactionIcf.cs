using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxTransactionIcf
{
    public decimal TrId { get; set; }

    public DateTime? TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public decimal? TrTtype { get; set; }

    public decimal? TrCardNumber { get; set; }

    public string? TrCsnnumber { get; set; }

    public string? TrIpAddress { get; set; }

    public string? TrMessage { get; set; }

    public string? TrEmpId { get; set; }

    public DateTime? TrCreated { get; set; }
}
