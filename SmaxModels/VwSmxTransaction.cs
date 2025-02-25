using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxTransaction
{
    public string? EmpId { get; set; }

    public string? Name { get; set; }

    public string? ChLname { get; set; }

    public string? CardNumber { get; set; }

    public string? Date { get; set; }

    public string? Time { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? TrTtype { get; set; }

    public int DeLnId { get; set; }

    public string LnName { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string DeviceName { get; set; } = null!;
}
