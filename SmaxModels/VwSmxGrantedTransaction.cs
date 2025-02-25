using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxGrantedTransaction
{
    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? CardNumber { get; set; }

    public DateOnly? TrDate { get; set; }

    public string? Date { get; set; }

    public string? Time { get; set; }

    public decimal? TrTtype { get; set; }

    public int DeLnId { get; set; }

    public string LnName { get; set; } = null!;

    public string ReaderType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string DeviceName { get; set; } = null!;
}
