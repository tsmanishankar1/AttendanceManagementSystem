using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxGrantedTransactionApix
{
    public decimal? TrId { get; set; }

    public string EmpId { get; set; } = null!;

    public string? Date { get; set; }

    public string? Time { get; set; }

    public string ReaderType { get; set; } = null!;

    public string Message { get; set; } = null!;
}
